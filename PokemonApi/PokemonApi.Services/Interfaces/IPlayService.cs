using PokemonApi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonApi.Services.Interfaces
{
    public interface IPlayService
    {
        public Task<PokemonEntity> CatchPokemon(Guid locationId, Guid userId);

        public Task<string> BattlePokemon(Guid locationId,int index, Guid userId);

    }
}
