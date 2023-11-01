using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonApi.Data.Models.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public ICollection<PokemonEntity> Pokemons { get; set; } = new HashSet<PokemonEntity>();
    }
}
