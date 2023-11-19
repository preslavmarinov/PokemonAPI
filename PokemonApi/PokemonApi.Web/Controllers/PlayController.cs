using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokemonApi.Common;
using PokemonApi.Services.Interfaces;
using PokemonApi.Web.Controllers.Abstract;
using PokemonApi.Web.Models.Play;
using PokemonApi.Web.Models.Pokemon;
using System.Security.Claims;

namespace PokemonApi.Web.Controllers
{
    [Authorize(PolicyNames.USER_AND_ABOVE)]
    public class PlayController : ApiBaseController
    {
        private readonly IPlayService _playService;
        private readonly ILocationService _locationService;
        private readonly IMapper _mapper;

        public PlayController(IPlayService playService, ILocationService locationService, IMapper mapper)
        {
            this._playService = playService;
            this._locationService = locationService;
            this._mapper = mapper;
        }

        [HttpPut("catch/pokemon/{locationId}")]
        public async Task<IActionResult> CatchPokemon([FromRoute] Guid locationId)
        {
            var exists = await this._locationService.ExistsAsync(locationId);
            if (!exists)
            {
                return NotFound("Location not found");
            }
            var pokemon = await _playService.CatchPokemon(locationId, Guid.Parse(getLoggedInUserId()));

            if(pokemon == null)
            {
                return Ok("Better luck next time");
            }
            var pokemonViewModel = this._mapper.Map<PokemonViewModel>(pokemon);

            return Ok(pokemonViewModel);
        }

        [HttpPost("battle")]
        public async Task<IActionResult> BattlePokemon(BattleModel model)
        {
            var exists = await this._locationService.ExistsAsync(model.LocationId);
            if (!exists)
            {
                return NotFound("Location not found");
            }


            string result;
            try
            {
                result = await this._playService.BattlePokemon(model.LocationId, model.PokemonIndex, Guid.Parse(getLoggedInUserId()));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(result);
        }

        private string getLoggedInUserId()
        {
            return User.FindFirst("UserID").Value;
        }
    }
}
