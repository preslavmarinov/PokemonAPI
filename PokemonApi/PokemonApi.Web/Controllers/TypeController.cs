using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokemonApi.Common;
using PokemonApi.Data.Models;
using PokemonApi.Services.Interfaces;
using PokemonApi.Web.Controllers.Abstract;
using PokemonApi.Web.Models.Location;
using PokemonApi.Web.Models.Pokemon;
using PokemonApi.Web.Models.Type;
using System.ComponentModel.DataAnnotations;

namespace PokemonApi.Web.Controllers
{
    [Authorize(PolicyNames.USER_AND_ABOVE)]
    public class TypeController : ApiBaseController
    {
        private readonly ITypeService _typeService;

        public TypeController(ITypeService typeService)
        {
            this._typeService = typeService;
        }

        [HttpGet("type/{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var type = await this._typeService.GetTypeByIdAsync(id, x => new TypeViewInputModel
            {
                Name = x.Name,
            });

            if (type == null)
            {
                return NotFound();
            }

            return this.Ok(type);
        }

        [HttpGet("types")]
        public async Task<IActionResult> GetTypes()
        {
            var types = await this._typeService.GetTypesAsync(x => new TypeViewInputModel { Name = x.Name });

            return this.Ok(types);
        }

        [HttpGet("type/pokemons/{typeId}")]
        public async Task<IActionResult> GetPokemonsByType(
            [FromRoute] Guid typeId,
            [FromQuery][Range(0, int.MaxValue)] int page,
            [FromQuery][Range(0, 50)] int perPage)
        {
            var pokemons = await this._typeService.GetPokemonsByType(typeId, page, perPage, x => new PokemonViewModel
            {
                Name = x.Name,
                HP = x.HP.ToString(),
                Attack = x.Attack.ToString(),
                Defence = x.Defence.ToString(),
                Speed = x.Speed.ToString(),
                Generation = x.Generation.ToString(),
                IsLegendary = x.IsLegendary.ToString(),
                Types = x.Types.Select(y => new TypeViewInputModel { Name = y.Type.Name }).ToArray(),
                Location = new LocationViewInputModel { Name = x.Location.Name },
                Owner = x.ApplicationUser != null ? x.ApplicationUser.Email : null,
            });

            return this.Ok(pokemons);
        }

        [HttpPost("type")]
        [Authorize(Roles = RoleNames.ADMIN)]
        public async Task<IActionResult> CreateType(TypeViewInputModel type)
        {
            var newType = new TypeEntity { Name = type.Name };
            Guid id = await this._typeService.CreateTypeAsync(newType);

            return CreatedAtAction(
                nameof(this.Get),
                new { id = id.ToString() }
               );
        }

        [HttpPut("type/{id}")]
        [Authorize(Roles = RoleNames.ADMIN)]
        public async Task<IActionResult> UpdateType([FromRoute] Guid id, TypeViewInputModel type)
        {
            //TODO
            bool exists = await this._typeService.ExistsAsync(id);

            if (!exists)
            {
                return this.NotFound();
            }

            var updatedType = new TypeEntity { Name = type.Name };

            var existingType = await this._typeService.UpdateTypeAsync(id, updatedType);

            return this.Ok(existingType);
        }

        [HttpDelete("type/{id}")]
        [Authorize(Roles = RoleNames.ADMIN)]
        public async Task<IActionResult> DeleteType(Guid id)
        {
            bool exists = await this._typeService.ExistsAsync(id);

            if (!exists)
            {
                return this.NotFound();
            }

            await this._typeService.DeleteTypeAsync(id);

            return this.Ok();
        }
    }
}
