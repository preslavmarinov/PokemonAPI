using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using PokemonApi.Common;
using PokemonApi.Common.Configurations;
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
        private readonly IMemoryCache _memoryCache;
        private readonly IConfiguration _configuration;

        public TypeController(ITypeService typeService, IMemoryCache memoryCache, IConfiguration configuration)
        {
            this._typeService = typeService;
            this._memoryCache = memoryCache;
            this._configuration = configuration;
        }

        [HttpGet("type/{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var type = await this._typeService.GetTypeByIdAsync(id, x => new TypeViewModel
            {
                Id = x.Id,
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
            bool isRequestCached = this._memoryCache.TryGetValue(CacheKeys.TYPES_CACHE_KEY, out IEnumerable<TypeViewModel> cachedData);

            if (isRequestCached)
            {
                return this.Ok(cachedData);
            }

            var types = await this._typeService.GetTypesAsync(x => new TypeViewModel { Id = x.Id, Name = x.Name });

            long expiration = this._configuration.GetSection("Cache").Get<CacheConfiguration>().Duration;

            this._memoryCache.Set(
                CacheKeys.TYPES_CACHE_KEY,
                types,
                new MemoryCacheEntryOptions()
                    .SetSize(types.LongCount())
                    .SetAbsoluteExpiration(
                        TimeSpan.FromSeconds(expiration)));

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
                Types = x.Types.Select(y => new TypeViewModel { Name = y.Type.Name }).ToArray(),
                Location = new LocationViewModel { Name = x.Location.Name },
                Owner = x.ApplicationUser != null ? x.ApplicationUser.Email : null,
            });

            return this.Ok(pokemons);
        }

        [HttpPost("type")]
        [Authorize(Roles = RoleNames.ADMIN)]
        public async Task<IActionResult> CreateType(TypeInputModel type)
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
        public async Task<IActionResult> UpdateType([FromRoute] Guid id, TypeInputModel type)
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
