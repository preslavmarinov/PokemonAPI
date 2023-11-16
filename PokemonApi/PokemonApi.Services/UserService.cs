using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PokemonApi.Data;
using PokemonApi.Data.Models;
using PokemonApi.Data.Models.Identity;
using PokemonApi.Services.Interfaces;
using System.Linq.Expressions;

namespace PokemonApi.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly PokemonDbContext _context;

        public UserService(UserManager<ApplicationUser> userManager, PokemonDbContext context)
        {
            this._userManager = userManager;
            this._context = context;
        }

        public async Task<Guid> CreateUserAsync(ApplicationUser user, string password, string role)
        {
            var result = await _userManager.CreateAsync(user, password);

            await _userManager.AddToRoleAsync(user, role);

            return user.Id;
        }

        public async Task DeleteUserAsync(Guid id)
        {
            var user = await GetUserByIdAsync(id);
            var userRoles = await this._userManager.GetRolesAsync(user);

            foreach (var role in userRoles)
            {
                await _userManager.RemoveFromRoleAsync(user, role);
            }

            await this._userManager.DeleteAsync(user);
            await this._context.SaveChangesAsync();
        }

        public async Task<ApplicationUser> GetUserByIdAsync(Guid id)
        {
            return await this._userManager.FindByIdAsync(id.ToString());
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsersAsync()
        {
            return await this._userManager.Users.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetUserPokemonsAsync<T>(Guid id, Expression<Func<PokemonEntity, T>> selector)
        {
            return await this._context.Pokemons.Where(x => x.ApplicationUserId == id).Select(selector).ToListAsync();
        }

        public async Task<ApplicationUser> UpdateUserAsync(Guid id, ApplicationUser user)
        {
            var existingUser = await this.GetUserByIdAsync(id);
            existingUser.UserName = user.UserName;
            existingUser.Email = user.Email;
            existingUser.NormalizedUserName= user.NormalizedUserName;

            await this._userManager.UpdateAsync(existingUser);

            return existingUser;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            var user = await this._userManager.FindByIdAsync(id.ToString());
            if (user != null)
            {
                return true;
            }

            return false;
        }
        
    }
}
