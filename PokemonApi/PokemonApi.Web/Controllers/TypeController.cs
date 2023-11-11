using Microsoft.AspNetCore.Mvc;
using PokemonApi.Data.Models;
using PokemonApi.Services.Interfaces;
using PokemonApi.Web.Controllers.Abstract;
using PokemonApi.Web.Models.Location;
using PokemonApi.Web.Models.Type;

namespace PokemonApi.Web.Controllers
{
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

        [HttpPost("type")]
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
        public async Task<IActionResult> UpdateType([FromRoute] Guid id, TypeViewInputModel type)
        {
            bool exists = await this._typeService.ExistsAsync(id);

            if (!exists)
            {
                return this.NotFound();
            }

            var updatedType = new TypeEntity { Name = type.Name };

            await this._typeService.UpdateTypeAsync(updatedType);

            return this.Ok(updatedType);
        }

        [HttpDelete("type/{id}")]
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
