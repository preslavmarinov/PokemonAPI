using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonApi.Common.Enums;
using PokemonApi.Data.Models.Identity;
using PokemonApi.Services.Interfaces;
using PokemonApi.Web.Controllers.Abstract;
using PokemonApi.Web.Models.Location;
using PokemonApi.Web.Models.Pokemon;
using PokemonApi.Web.Models.Type;
using PokemonApi.Web.Models.User;
using System.ComponentModel.DataAnnotations;

namespace PokemonApi.Web.Controllers
{
    public class UserController : ApiBaseController
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet("user/{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var user = await this._userService.GetUserByIdAsync(id);

            if( user == null)
            {
                return NotFound();
            }

            var userViewModel = this._mapper.Map<UserViewModel>(user);

            return Ok(userViewModel);
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await this._userService.GetUsersAsync();

            var userViewModels = this._mapper.Map<UserViewModel>(users);

            return Ok(userViewModels);
        }

        [HttpGet("user/pokemons/{userId}")]
        public async Task<IActionResult> GetUserPokemons([FromRoute] Guid userId)
        {
            var pokemons = await this._userService.GetUserPokemonsAsync(userId, x => new PokemonViewModel
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

            return Ok(pokemons);
        }

        [HttpPost("user")]
        public async Task<IActionResult> CreateUser(
            [FromQuery][EnumDataType(typeof(Role))] Role role,
            UserInputModel user)
        {
            var newUser = new ApplicationUser
            {
                UserName = user.Username,
                NormalizedUserName = user.Username.ToUpper(),
                Email = user.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            Guid id = await this._userService.CreateUserAsync(newUser, user.Password, role.ToString());

            return CreatedAtAction(
                nameof(this.Get),
                new { id = id.ToString() }
                );
        }

        [HttpPut("user/{id}")]
        public async Task<IActionResult> UpdateUser(
            [FromRoute] Guid id,
            UserInputModel user)
        {
            bool exists = await this._userService.ExistsAsync(id);

            if (!exists)
            {
                return NotFound();
            }

            var updatedUser = new ApplicationUser 
            {
                UserName = user.Username,
                NormalizedUserName = user.Username.ToUpper(),
                Email = user.Email,
            };

            var existingUser = await this._userService.UpdateUserAsync(id, updatedUser);

            return Ok(existingUser);
        }

        [HttpDelete("user/{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
        {
            bool exists = await this._userService.ExistsAsync(id);

            if(!exists)
            {
                return NotFound();
            }

            await this._userService.DeleteUserAsync(id);

            return Ok();
        }
    }
}
