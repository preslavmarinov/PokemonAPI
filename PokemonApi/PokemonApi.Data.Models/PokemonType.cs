using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonApi.Data.Models
{
    public class PokemonType
    {
        public Guid PokemonId { get; set; }
        public PokemonEntity Pokemon { get; set; }
        public Guid TypeId { get; set; }
        public TypeEntity Type { get; set; }

    }
}
