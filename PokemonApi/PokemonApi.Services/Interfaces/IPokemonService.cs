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

        public Task<PokemonEntity> UpdatePokemonAsync(Guid id, PokemonEntity pokemon);

        public Task<IEnumerable<PokemonEntity>> GetPokemonsAsync(int page, int perPage, string sortAttr, string sortDir);

        public Task<T?> GetPokemonByIdAsync<T>(Guid id, Expression<Func<PokemonEntity, T>> selector);

        public Task<bool> ExistsAsync(Guid id);
    }
}
