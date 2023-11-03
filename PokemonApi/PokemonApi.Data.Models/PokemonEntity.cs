using PokemonApi.Data.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonApi.Data.Models
{
    public class PokemonEntity : BaseEntity
    {
        public string Name { get; set; }
        public int HP { get; set; }
        public int Attack { get; set; }
        public int Defence { get; set; }
        public int Speed { get; set; }
        public int Generation { get; set; }
        public bool IsLegendary { get; set; }
        public ICollection<PokemonType> Types { get; set; } = new HashSet<PokemonType>();
        public Guid TypeId { get; set; }
        public TypeEntity Type { get; set; }
        public Guid? ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
    }
}
