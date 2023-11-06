using Microsoft.AspNetCore.Mvc;
using PokemonApi.Services.Interfaces;

namespace PokemonApi.Web.Controllers
{
    public class AuthorizationController : ControllerBase
    {
        private readonly IAuthorizationService _authorizationService;

        public AuthorizationController(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register()
        {
            var result = await this._authorizationService.RegisterAsync("SSsdhs@gmail.com", "ss123sj");

            return this.Ok(result);
        }


        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login()
        {
            var token = await this._authorizationService.LoginAsync("sdhs", "ss123sj");

            if(token == null)
            {
                return this.Unauthorized();
            }

            return this.Ok(token);
        }
    }
}
