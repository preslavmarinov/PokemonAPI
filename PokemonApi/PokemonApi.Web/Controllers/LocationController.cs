using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using PokemonApi.Common;
using PokemonApi.Common.Configurations;
using PokemonApi.Data.Models;
using PokemonApi.Services.Interfaces;
using PokemonApi.Web.Controllers.Abstract;
using PokemonApi.Web.Models.Location;
using PokemonApi.Web.Models.Pokemon;
using PokemonApi.Web.Models.Type;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace PokemonApi.Web.Controllers
{
    [Authorize(PolicyNames.USER_AND_ABOVE)]
    public class LocationController : ApiBaseController
    {
        private readonly ILocationService _locationService;
        private readonly IMemoryCache _memoryCache;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public LocationController(
            ILocationService locationService,
            IMemoryCache memoryCache,
            IConfiguration configuration,
            IMapper mapper)
        {
            this._locationService = locationService;
            this._memoryCache = memoryCache;
            this._configuration = configuration;
            this._mapper = mapper;
        }

        [HttpGet("location/{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var location = await this._locationService.GetLocationByIdAsync(id, x => new LocationViewModel
            {
                Id = x.Id,
                Name = x.Name,
            });

            if (location == null)
            {
                return NotFound();
            }

            return this.Ok(location);
        }

        [HttpGet("locations")]
        public async Task<IActionResult> GetLocations()
        {
            bool isRequestCached = this._memoryCache.TryGetValue(CacheKeys.LOCATIONS_CACHE_KEY, out IEnumerable<LocationViewModel> cachedData);

            if (isRequestCached)
            {
                return this.Ok(cachedData);
            }

            var locations = await this._locationService.GetLocationsAsync(x => new LocationViewModel { Id = x.Id, Name = x.Name });

            long expiration = this._configuration.GetSection("Cache").Get<CacheConfiguration>().Duration;

            this._memoryCache.Set(
                CacheKeys.LOCATIONS_CACHE_KEY,
                locations,
                new MemoryCacheEntryOptions()
                    .SetSize(locations.LongCount())
                    .SetAbsoluteExpiration(
                        TimeSpan.FromSeconds(expiration)));

            return this.Ok(locations);
        }

        [HttpGet("location/pokemons/{locationId}")]
        public async Task<IActionResult> GetPokemonsFromLocation(
            [FromRoute] Guid locationId,
            [FromQuery][Range(0, int.MaxValue)] int page,
            [FromQuery][Range(0, 50)] int perPage)
        {
            var pokemons = await this._locationService.GetPokemonsFromLocation(locationId, page, perPage, x => new PokemonViewModel
            {
                Id = x.Id,
                Name = x.Name,
                HP = x.HP.ToString(),
                Attack = x.Attack.ToString(),
                Defence = x.Defence.ToString(),
                Speed = x.Speed.ToString(),
                Generation = x.Generation.ToString(),
                IsLegendary = x.IsLegendary.ToString(),
                Types = x.Types.Select(y => new TypeViewModel {Id = y.Type.Id, Name = y.Type.Name }).ToArray(),
                Location = new LocationViewModel { Id = x.Location.Id, Name = x.Location.Name },
                Owner = x.ApplicationUser != null ? x.ApplicationUser.Email : null,
            });

            return this.Ok(pokemons);
        }

        [HttpPost("location")]
        [Authorize(Roles = RoleNames.ADMIN)]
        public async Task<IActionResult> CreateLocation(LocationInputModel location)
        {
            var newLocation = new LocationEntity { Name = location.Name };
            Guid id = await this._locationService.CreateLocationAsync(newLocation);

            return CreatedAtAction(
                nameof(this.Get),
                new { id = id.ToString() }
               );
        }

        [HttpPut("location/{id}")]
        [Authorize(Roles = RoleNames.ADMIN)]
        public async Task<IActionResult> UpdateLocation([FromRoute] Guid id, LocationInputModel location)
        {
            bool exists = await this._locationService.ExistsAsync(id);

            if (!exists)
            {
                return this.NotFound();
            }

            var updatedlocation = new LocationEntity { Name = location.Name };

            var existingLocation = await this._locationService.UpdateLocationAsync(id,updatedlocation);
            var result = this._mapper.Map<LocationViewModel>(existingLocation);

            return this.Ok(result);
        }

        [HttpDelete("location/{id}")]
        [Authorize(Roles = RoleNames.ADMIN)]
        public async Task<IActionResult> DeleteLocation(Guid id)
        {
            bool exists = await this._locationService.ExistsAsync(id);

            if (!exists)
            {
                return this.NotFound();
            }

            await this._locationService.DeleteLocationAsync(id);

            return this.Ok();
        }
    }
}
