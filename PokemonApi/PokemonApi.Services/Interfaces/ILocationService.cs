
using PokemonApi.Data.Models;
using System.Linq.Expressions;

namespace PokemonApi.Services.Interfaces
{
    public interface ILocationService
    {
        public Task<Guid> CreateLocationAsync(LocationEntity location);

        public Task DeleteLocationAsync(Guid id);

        public Task<LocationEntity> UpdateLocationAsync(LocationEntity location);

        public Task<IEnumerable<T>> GetLocationsAsync<T>(Expression<Func<LocationEntity, T>> selector);

        public Task<T?> GetLocationByIdAsync<T>(Guid id, Expression<Func<LocationEntity, T>> selector);

        public Task<bool> ExistsAsync(Guid id);

        public Task<IEnumerable<T>> GetPokemonsFromLocation<T>(Guid id, int page, int perPage, Expression<Func<PokemonEntity, T>> selector);
    }
}
