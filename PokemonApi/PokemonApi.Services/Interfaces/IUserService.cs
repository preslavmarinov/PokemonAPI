using PokemonApi.Data.Models;
using PokemonApi.Data.Models.Identity;
using System.Linq.Expressions;


namespace PokemonApi.Services.Interfaces
{
    public interface IUserService
    {
        public Task<IEnumerable<ApplicationUser>> GetUsersAsync();

        public Task<ApplicationUser> GetUserByIdAsync(Guid id);

        public Task<IEnumerable<T>> GetUserPokemonsAsync<T>(Guid id, Expression<Func<PokemonEntity, T>> selector);

        public Task<Guid> CreateUserAsync(ApplicationUser user, string password, string role);

        public Task DeleteUserAsync(Guid id);

        public Task<ApplicationUser> UpdateUserAsync(Guid id, ApplicationUser user);

        public Task<bool> ExistsAsync(Guid id);
    }
}
