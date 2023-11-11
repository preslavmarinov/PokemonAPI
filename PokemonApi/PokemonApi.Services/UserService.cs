using Microsoft.AspNetCore.Identity;
using PokemonApi.Data;
using PokemonApi.Data.Models.Identity;
using PokemonApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<Guid> CreateUserAsync(ApplicationUser user)
        {
            return Guid.NewGuid();
            //var newUser = new ApplicationUser
            //{
            //    UserName = user.Email,
            //    NormalizedUserName = user.Email.ToUpper(),
            //    Email = user.Email,
            //    SecurityStamp = Guid.NewGuid().ToString(),
            //};

            //var result = await _userManager.CreateAsync(newUser, user.PasswordHash);

            //if (!result.Succeeded)
            //{
            //    return result;
            //}
            //await _userManager.AddToRoleAsync(newUser, "user");

            //return result;
        }

        public async Task DeleteUserAsync(Guid id)
        {
            //var user = await GetUserByIdAsync(id, ApplicationUser);

            //this._userManager.DeleteAsync();
        }

        public Task<T> GetUserByIdAsync<T>(Guid id, System.Linq.Expressions.Expression<Func<ApplicationUser, T>> selector)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetUsersAsync<T>(System.Linq.Expressions.Expression<Func<ApplicationUser, T>> selector)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUser> UpdateUserAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
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
