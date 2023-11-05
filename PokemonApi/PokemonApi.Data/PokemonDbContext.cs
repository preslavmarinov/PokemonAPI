using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PokemonApi.Data.Models;
using PokemonApi.Data.Models.Identity;

namespace PokemonApi.Data
{
    public class PokemonDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public PokemonDbContext(DbContextOptions<PokemonDbContext> options)
            :base(options)
        {
        }

        public DbSet<PokemonEntity> Pokemons { get; set; } = default!;

        public DbSet<LocationEntity> Locations { get; set; } = default!;

        public DbSet<TypeEntity> Types { get; set; } = default!;

        public DbSet<PokemonType> PokemonTypes { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<PokemonType>().HasKey(x => new { x.PokemonId, x.TypeId});
            builder.Entity<PokemonType>().HasOne(x => x.Pokemon).WithMany(x => x.Types).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<PokemonType>().HasOne(x => x.Type).WithMany(x => x.Pokemons).OnDelete(DeleteBehavior.NoAction);
        }
    }


}
