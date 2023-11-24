using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokemonApi.Common;
using PokemonApi.Services.Interfaces;
using PokemonApi.Web.Controllers.Abstract;
using PokemonApi.Web.Models.Location;
using PokemonApi.Web.Models.Play;
using PokemonApi.Web.Models.Pokemon;
using PokemonApi.Web.Models.Type;
using PokemonApi.Web.Pdf;
using System.Security.Claims;

namespace PokemonApi.Web.Controllers
{
    [Authorize(PolicyNames.USER_AND_ABOVE)]
    public class PlayController : ApiBaseController
    {
        private readonly IPlayService _playService;
        private readonly ILocationService _locationService;
        private readonly IUserService _userService;
        private readonly IPdfService _pdfService;
        private readonly IMapper _mapper;

        public PlayController(
            IPlayService playService,
            ILocationService locationService,
            IUserService userService,
            IPdfService pdfService,
            IMapper mapper)
        {
            this._playService = playService;
            this._locationService = locationService;
            this._userService = userService;
            this._pdfService = pdfService;
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

        [HttpPost("battle/pokemon")]
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

        [HttpGet("userInfo")]
        public async Task<IActionResult> GetUserInfo()
        {
            Guid userId = Guid.Parse(getLoggedInUserId());
            var pokemons = await this._userService.GetUserPokemonsAsync(userId, x => new PokemonViewModel
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

            var user = await this._userService.GetUserByIdAsync(userId);
            UserInfoModel userInfo = new UserInfoModel
            {
                User = user,
                Pokemons= pokemons,
            };

            var pdfFile = this._pdfService.GetPdf(new UserInfoPdf(userInfo));              

            return this.File(pdfFile, "application/pdf", string.Format(CacheKeys.USER_INFO_CACHE_KEY, userId), true);
        }

        private string getLoggedInUserId()
        {
            return User.FindFirst("UserID").Value;
        }
    }
}
