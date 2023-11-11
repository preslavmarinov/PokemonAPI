using Microsoft.AspNetCore.Mvc;
using PokemonApi.Data.Models;
using PokemonApi.Services.Interfaces;
using PokemonApi.Web.Controllers.Abstract;
using PokemonApi.Web.Models.Location;

namespace PokemonApi.Web.Controllers
{
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

        [HttpPost("location")]
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
        public async Task<IActionResult> UpdateLocation([FromRoute] Guid id, LocationViewInputModel location)
        {
            bool exists = await this._locationService.ExistsAsync(id);

            if (!exists)
            {
                return this.NotFound();
            }

            var updatedlocation = new LocationEntity { Name = location.Name };

            await this._locationService.UpdateLocationAsync(updatedlocation);

            return this.Ok(updatedlocation);
        }

        [HttpDelete("location/{id}")]
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
