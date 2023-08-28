using Microsoft.EntityFrameworkCore;
using PokedexApp.Models;

namespace PokedexApp.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Pokemon> Pokemons { get; set; }
        public DbSet<PokemonSprite> PokemonSprites { get; set; }
        public DbSet<Models.Type> Types { get; set; }

		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
		}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Type>()
                           .Property(t => t.Id)
                           .ValueGeneratedNever(); // Disable identity behavior

			modelBuilder.Entity<Pokemon>()
			            .Property(p => p.Id)
			            .ValueGeneratedNever(); // Disable identity behavior
		}

	}
}
