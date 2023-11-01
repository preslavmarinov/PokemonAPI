using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PokemonApi.Data.Models;
using PokemonApi.Data.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonApi.Data
{
    public class PokemonDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public PokemonDbContext(DbContextOptions<PokemonDbContext> options)
            :base(options)
        {
        }

        public DbSet<PokemonEntity> Pokemons { get; set; } = default!;

        public DbSet<LocationEntity> Location { get; set; } = default!;

        public DbSet<TypeEntity> Type { get; set; } = default!;
    }
}
