using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonApi.Data.Models
{
    public class LocationEntity : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<PokemonEntity> Pokemons { get; set; } = new HashSet<PokemonEntity>();
    }
}
