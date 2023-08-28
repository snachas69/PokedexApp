using PokedexApp.Data;
using PokedexApp.Models;
using System.Net.Http.Headers;

namespace PokedexApp.Service
{
    public class PokemonSpriteService : IService<PokemonSprite>
    {
        private AppDbContext _context;
        private IConfiguration _configuration;
        private HttpClient _client;

        public PokemonSpriteService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;

            _client = new HttpClient();
			_client.BaseAddress = new Uri(_configuration["ApiUrls:PokeApiBaseUrl"]);
			_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
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
