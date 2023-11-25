using PokemonApi.Data.Models;
using PokemonApi.Data;
using System.Linq.Expressions;
using PokemonApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace PokemonApi.Services
{
    public class TypeService : ITypeService
    {
        private readonly PokemonDbContext _context;

        public TypeService(PokemonDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreateTypeAsync(TypeEntity type)
        {
            this._context.Types.Add(type);

            await this._context.SaveChangesAsync();

            return type.Id;
        }

        public async Task DeleteTypeAsync(Guid id)
        {
            var pokemonTypes = await this._context.PokemonTypes.Where(x => x.TypeId == id).ToArrayAsync();
            this._context.PokemonTypes.RemoveRange(pokemonTypes);

            var type = this._context.Types.FirstOrDefault(x => x.Id == id);

            this._context.Types.Remove(type);

            await this._context.SaveChangesAsync();
        }

        public async Task<T?> GetTypeByIdAsync<T>(Guid id, Expression<Func<TypeEntity, T>> selector)
        {
            return await this._context.Types.Where(x => x.Id == id).Select(selector).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetTypesAsync<T>(Expression<Func<TypeEntity, T>> selector)
        {
            return await this._context.Types.Select(selector).ToListAsync();
        }

        public async Task<TypeEntity> UpdateTypeAsync(Guid id, TypeEntity updatedType)
        {
            var existingType = await this._context.Types.FindAsync(id);

            existingType.Name = updatedType.Name;

            this._context.Types.Update(existingType);

            await this._context.SaveChangesAsync();

            return existingType;
        }

        public async Task<bool> ExistsAsync(Guid id)
            => await this._context.Types.AnyAsync(x => x.Id == id);

        public async Task<IEnumerable<T>> GetPokemonsByType<T>(Guid id, int page, int perPage, Expression<Func<PokemonEntity, T>> selector)
        {
            var pokemonIds = await this._context.PokemonTypes.Where(x => x.TypeId == id).Select(x => x.PokemonId).ToListAsync();

            if (page == 0 || perPage == 0)
            {
                return await this._context.Pokemons.Where(x => pokemonIds.Contains(x.Id)).Select(selector).ToListAsync();
            }

            return await this._context.Pokemons
                .Where(x => pokemonIds.Contains(x.Id))
                .Skip((page-1)*perPage)
                .Take(perPage)
                .Select(selector).ToListAsync();
        }
    }
}
