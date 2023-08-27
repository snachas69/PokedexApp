using PokedexApp.Data;
using PokedexApp.Models;

namespace PokedexApp.Service
{
    public class PokemonSpriteService : IService<PokemonSprite>
    {
        private AppDbContext _context;

        public PokemonSpriteService(AppDbContext context)
        {
            _context = context;
        }

        public List<PokemonSprite> GetDataFromApi()
        {
            throw new NotImplementedException();
        }

		public IQueryable<PokemonSprite> GetDataFromDatabase()
		{
            throw new NotImplementedException();
		}
	}
}
