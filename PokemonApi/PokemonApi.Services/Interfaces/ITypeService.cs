using PokemonApi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PokemonApi.Services.Interfaces
{
    public interface ITypeService
    {
        public Task<Guid> CreateTypeAsync(TypeEntity location);

        public Task DeleteTypeAsync(Guid id);

        public Task<TypeEntity> UpdateTypeAsync(TypeEntity location);

        public Task<IEnumerable<T>> GetTypesAsync<T>(Expression<Func<TypeEntity, T>> selector);

        public Task<T?> GetTypeByIdAsync<T>(Guid id, Expression<Func<TypeEntity, T>> selector);

        public Task<bool> ExistsAsync(Guid id);
    }
}
