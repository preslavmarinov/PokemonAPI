using Microsoft.EntityFrameworkCore;
using PokemonApi.Data;
using PokemonApi.Data.Models;
using PokemonApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PokemonApi.Services
{
    public class PokemonService : IPokemonService
    {
        private readonly PokemonDbContext _context;

        public PokemonService(PokemonDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreatePokemonAsync(PokemonEntity pokemon)
        {
            this._context.Pokemons.Add(pokemon);

            await _context.SaveChangesAsync();

            return pokemon.Id;
        }

        public async Task DeletePokemonAsync(Guid id)
        {
            var pokemonTypes = await this._context.PokemonTypes.Where(x => x.PokemonId == id).ToArrayAsync();
            this._context.PokemonTypes.RemoveRange(pokemonTypes);

            var pokemon = await this._context.Pokemons.FirstOrDefaultAsync(x => x.Id == id);

            this._context.Pokemons.Remove(pokemon);

            await this._context.SaveChangesAsync();
        }

        public async Task<T?> GetPokemonByIdAsync<T>(Guid id, Expression<Func<PokemonEntity, T>> selector)
        {
            return await this._context.Pokemons.Where(x => x.Id == id).Select(selector).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetPokemonsAsync<T>(int page, int perPage, Expression<Func<PokemonEntity, T>> selector)
        {
            return await this._context.Pokemons.Skip((page-1)*perPage).Take(perPage).Select(selector).ToListAsync();
        }

        public async Task<PokemonEntity> UpdatePokemonAsync(PokemonEntity pokemon)
        {
            this._context.Pokemons.Update(pokemon);

            await this._context.SaveChangesAsync();

            return pokemon;
        }

        public async Task<bool> ExistsAsync(Guid id)
            => await this._context.Pokemons.AnyAsync(x => x.Id == id);
    }
}
