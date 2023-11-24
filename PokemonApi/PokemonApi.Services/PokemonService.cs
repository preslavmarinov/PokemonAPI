using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PokemonApi.Data;
using PokemonApi.Data.Models;
using PokemonApi.Data.Models.Identity;
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
        private readonly UserManager<ApplicationUser> _userManager;

        public PokemonService(PokemonDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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

        public async Task<IEnumerable<PokemonEntity>> GetPokemonsAsync(int page, int perPage, string sortAttr, string sortDir)
        {
            string sqlQuery = "SELECT * FROM Pokemons";

            string sortQuery = "";
            if (!string.IsNullOrEmpty(sortAttr) && !string.IsNullOrEmpty(sortDir))
            {
                sortQuery = $"ORDER BY {sortAttr} {sortDir}";
            }
            else
            {
                sortQuery = "ORDER BY (SELECT NULL)";
            }

            string pageQuery = $"OFFSET {(page - 1) * perPage} ROWS FETCH NEXT {perPage} ROWS ONLY";

            sqlQuery += $" {sortQuery} {pageQuery}";

            var pokemons = await this._context.Pokemons.FromSqlRaw(sqlQuery).ToListAsync();


            foreach( var pokemon in pokemons )
            {
                if(pokemon.LocationId != null)
                {
                    pokemon.Location = await this._context.Locations.Where(x => x.Id == pokemon.LocationId).FirstOrDefaultAsync();
                }

                var pokemonTypes = await this._context.PokemonTypes.Where(x => x.PokemonId == pokemon.Id).ToListAsync();
                pokemon.Types = pokemonTypes;
                foreach( var type in pokemonTypes )
                {
                    type.Type = await this._context.Types.FirstOrDefaultAsync(x => x.Id == type.TypeId);
                }

                if(pokemon.ApplicationUserId != null)
                {
                    pokemon.ApplicationUser = await this._userManager.FindByIdAsync(pokemon.ApplicationUserId.ToString());
                }
            }

            return pokemons;
        }

        public async Task<PokemonEntity> UpdatePokemonAsync(Guid id, PokemonEntity updatedPokemon)
        {
            var existingPokemon = await this._context.Pokemons.FindAsync(id);
            existingPokemon.Name = updatedPokemon.Name;
            existingPokemon.HP= updatedPokemon.HP;
            existingPokemon.Attack = updatedPokemon.Attack;
            existingPokemon.Defence = updatedPokemon.Defence;
            existingPokemon.Speed = updatedPokemon.Speed;
            existingPokemon.Generation = updatedPokemon.Generation;
            existingPokemon.IsLegendary = updatedPokemon.IsLegendary;
            existingPokemon.Types = updatedPokemon.Types;
            existingPokemon.LocationId = updatedPokemon.LocationId;

            this._context.Pokemons.Update(existingPokemon);

            await this._context.SaveChangesAsync();

            return existingPokemon;
        }

        public async Task<bool> ExistsAsync(Guid id)
            => await this._context.Pokemons.AnyAsync(x => x.Id == id);
    }
}
