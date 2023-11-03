using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonApi.Data.Models
{
    public class TypeEntity : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<PokemonType> Pokemons { get; set; } = new HashSet<PokemonType>();
    }
}
