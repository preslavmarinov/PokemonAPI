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
using System.Data;

namespace PokemonApi.Web.Controllers
{
    [Authorize(PolicyNames.USER_AND_ABOVE)]
    public class LocationController : ApiBaseController
    {
        private readonly ILocationService _locationService;

        public LocationController(ILocationService locationService)
        {
            this._locationService = locationService;
        }

        [HttpGet("location/{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var location = await this._locationService.GetLocationByIdAsync(id, x => new LocationViewInputModel
            {
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
            var locations = await this._locationService.GetLocationsAsync(x => new LocationViewInputModel { Name = x.Name });

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

        [HttpPost("location")]
        [Authorize(Roles = RoleNames.ADMIN)]
        public async Task<IActionResult> CreateLocation(LocationViewInputModel location)
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
        public async Task<IActionResult> UpdateLocation([FromRoute] Guid id, LocationViewInputModel location)
        {
            bool exists = await this._locationService.ExistsAsync(id);

            if (!exists)
            {
                return this.NotFound();
            }

            var updatedlocation = new LocationEntity { Name = location.Name };

            var existingLocation = await this._locationService.UpdateLocationAsync(id,updatedlocation);

            return this.Ok(existingLocation);
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
