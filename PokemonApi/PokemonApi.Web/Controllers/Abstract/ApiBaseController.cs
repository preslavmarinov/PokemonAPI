using Microsoft.AspNetCore.Mvc;

namespace PokemonApi.Web.Controllers.Abstract
{
    [ApiController]
    [Route("pokemonapi/v1")]
    public abstract class ApiBaseController : ControllerBase
    {
    }
}
