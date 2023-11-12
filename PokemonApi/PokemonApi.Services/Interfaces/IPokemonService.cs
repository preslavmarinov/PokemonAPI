using PokemonApi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PokemonApi.Services.Interfaces
{
    public interface IPokemonService
    {
        public Task<Guid> CreatePokemonAsync(PokemonEntity pokemon);

        public Task DeletePokemonAsync(Guid id);

        public Task<PokemonEntity> UpdatePokemonAsync(PokemonEntity pokemon);

        public Task<IEnumerable<T>> GetPokemonsAsync<T>(int page, int perPage, Expression<Func<PokemonEntity, T>> selector);

        public Task<T?> GetPokemonByIdAsync<T>(Guid id, Expression<Func<PokemonEntity, T>> selector);

        public Task<bool> ExistsAsync(Guid id);
    }
}
