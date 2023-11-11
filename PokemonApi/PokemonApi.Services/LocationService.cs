using Microsoft.EntityFrameworkCore;
using PokemonApi.Data;
using PokemonApi.Data.Models;
using PokemonApi.Services.Interfaces;
using System.Linq.Expressions;

namespace PokemonApi.Services
{
    public class LocationService : ILocationService
    {
        private readonly PokemonDbContext _context;

        public LocationService(PokemonDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreateLocationAsync(LocationEntity location)
        {
            this._context.Locations.Add(location);

            await this._context.SaveChangesAsync();

            return location.Id;
        }

        public async Task DeleteLocationAsync(Guid id)
        {
            var location = this._context.Locations.FirstOrDefault(x => x.Id == id);

            this._context.Locations.Remove(location);

            await this._context.SaveChangesAsync();
        }

        public async Task<T?> GetLocationByIdAsync<T>(Guid id, Expression<Func<LocationEntity, T>> selector)
        {
            return await this._context.Locations.Where(x => x.Id == id).Select(selector).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetLocationsAsync<T>(Expression<Func<LocationEntity, T>> selector)
        {
            return await this._context.Locations.Select(selector).ToListAsync();
        }

        public async Task<LocationEntity> UpdateLocationAsync(LocationEntity location)
        {
            this._context.Locations.Update(location);

            await this._context.SaveChangesAsync();

            return location;
        }

        public async Task<bool> ExistsAsync(Guid id)
            => await this._context.Locations.AnyAsync(x => x.Id == id);
    }
}
