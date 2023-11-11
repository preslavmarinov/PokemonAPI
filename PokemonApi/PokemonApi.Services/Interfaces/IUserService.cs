using PokemonApi.Data.Models.Identity;
using System.Linq.Expressions;


namespace PokemonApi.Services.Interfaces
{
    public interface IUserService
    {
        public Task<IEnumerable<T>> GetUsersAsync<T>(Expression<Func<ApplicationUser, T>> selector);

        public Task<T> GetUserByIdAsync<T>(Guid id, Expression<Func<ApplicationUser, T>> selector); 

        public Task<Guid> CreateUserAsync(ApplicationUser user);

        public Task DeleteUserAsync(Guid id);

        public Task<ApplicationUser> UpdateUserAsync(ApplicationUser user);

        public Task<bool> ExistsAsync(Guid id);
    }
}
