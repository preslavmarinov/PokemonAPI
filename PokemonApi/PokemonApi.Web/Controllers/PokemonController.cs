using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokemonApi.Common;
using PokemonApi.Common.Enums;
using PokemonApi.Data.Models;
using PokemonApi.Services.Interfaces;
using PokemonApi.Services.Seeding.DTO;
using PokemonApi.Web.Controllers.Abstract;
using PokemonApi.Web.Models.Location;
using PokemonApi.Web.Models.Pokemon;
using PokemonApi.Web.Models.Type;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace PokemonApi.Web.Controllers
{
    [Authorize(PolicyNames.USER_AND_ABOVE)]
    public class PokemonController : ApiBaseController
    {
        private readonly IPokemonService _pokemonService;
        private readonly IMapper _mapper;

        public PokemonController(IPokemonService pokemonService, IMapper mapper)
        {
            this._pokemonService = pokemonService;
            this._mapper = mapper;
        }

        [HttpGet("pokemon/{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var pokemon = await this._pokemonService.GetPokemonByIdAsync(id,x => new PokemonViewModel
            {
                Id = x.Id,
                Name = x.Name,
                HP = x.HP.ToString(),
                Attack = x.Attack.ToString(),
                Defence = x.Defence.ToString(),
                Speed = x.Speed.ToString(),
                Generation = x.Generation.ToString(),
                IsLegendary = x.IsLegendary.ToString(),
                Types = x.Types.Select(y => new TypeViewModel { Name = y.Type.Name}).ToArray(),
                Location = new LocationViewModel { Name = x.Location.Name },
                Owner = x.ApplicationUser!=null ? x.ApplicationUser.Email : null,
            });

            if (pokemon == null)
            {
                return NotFound();
            }

            return this.Ok(pokemon);
        }

        [HttpGet("pokemons")]
        public async Task<IActionResult> GetPokemons(
            [FromQuery][Range(0, int.MaxValue)] int page=1,
            [FromQuery][Range(0, 100)] int perPage = 5,
            [FromQuery][EnumDataType(typeof(SortAttribute))] SortAttribute? sortAttr=null,
            [FromQuery][EnumDataType(typeof(SortDirection))] SortDirection? sortDir=null)
        {
            var pokemonEntites = await this._pokemonService.GetPokemonsAsync(page, perPage, sortAttr.ToString(), sortDir.ToString());
            var pokemonViewModels = _mapper.Map<IEnumerable<PokemonViewModel>>(pokemonEntites);

            return this.Ok(pokemonViewModels);
        }

        [HttpPost("pokemon")]
        [Authorize(Roles = RoleNames.ADMIN)]
        public async Task<IActionResult> CreatePokemon(PokemonInputModel pokemon)
        {
            var newPokemon = new PokemonEntity
            {
                Name = pokemon.Name,
                HP = int.Parse(pokemon.HP),
                Attack = int.Parse(pokemon.Attack),
                Defence = int.Parse(pokemon.Defence),
                Speed = int.Parse(pokemon.Speed),
                Generation = int.Parse(pokemon.Generation),
                IsLegendary = bool.Parse(pokemon.IsLegendary),
                Types = pokemon.TypeIds.Select(x => new PokemonType { TypeId = x}).ToArray(),
                LocationId = pokemon.LocationId,
            };
            Guid id = await this._pokemonService.CreatePokemonAsync(newPokemon);

            return CreatedAtAction(
                nameof(this.Get),
                new { id = id.ToString() }
               );
        }

        [HttpPut("pokemon/{id}")]
        [Authorize(Roles = RoleNames.ADMIN)]
        public async Task<IActionResult> UpdatePokemon([FromRoute] Guid id, PokemonInputModel pokemon)
        {
            bool exists = await this._pokemonService.ExistsAsync(id);

            if (!exists)
            {
                return this.NotFound();
            }

            var updatedPokemon = new PokemonEntity
            {
                Name = pokemon.Name,
                HP = int.Parse(pokemon.HP),
                Attack = int.Parse(pokemon.Attack),
                Defence = int.Parse(pokemon.Defence),
                Speed = int.Parse(pokemon.Speed),
                Generation = int.Parse(pokemon.Generation),
                IsLegendary = bool.Parse(pokemon.IsLegendary),
                Types = pokemon.TypeIds.Select(x => new PokemonType { TypeId = x }).ToArray(),
                LocationId = pokemon.LocationId,
            };

            var existingPokemon = await this._pokemonService.UpdatePokemonAsync(id, updatedPokemon);

            return this.Ok(existingPokemon);
        }

        [HttpDelete("pokemon/{id}")]
        [Authorize(Roles = RoleNames.ADMIN)]
        public async Task<IActionResult> DeletePokemon(Guid id)
        {
            bool exists = await this._pokemonService.ExistsAsync(id);

            if (!exists)
            {
                return this.NotFound();
            }

            await this._pokemonService.DeletePokemonAsync(id);

            return this.Ok();
        }
    }
}
