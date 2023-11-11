using Microsoft.AspNetCore.Mvc;
using PokemonApi.Services.Interfaces;
using PokemonApi.Web.Controllers.Abstract;
using PokemonApi.Web.Models.User;

namespace PokemonApi.Web.Controllers
{
    public class AuthorizationController : ApiBaseController
    {
        private readonly IAuthorizationService _authorizationService;

        public AuthorizationController(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterUserModel user)
        {
            var result = await this._authorizationService.RegisterAsync(user.Email, user.Password);

            return this.Ok(result);
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginUserModel user)
        {
            var token = await this._authorizationService.LoginAsync(user.Email, user.Password);

            if(token == null)
            {
                return this.Unauthorized();
            }

            return this.Ok(token);
        }
    }
}
