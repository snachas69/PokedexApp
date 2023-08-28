using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PokedexApp.Data;
using PokedexApp.Models;
using PokedexApp.Service;

namespace PokedexApp.Controllers
{
    public class DiscoverController : Controller
    {
        private readonly AppDbContext _dbContext;
		private readonly IConfiguration _configuration;
		private static bool _isDataInDatabase;

        public DiscoverController(AppDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }
		
        static DiscoverController()
			=> _isDataInDatabase = false;

		public IActionResult Index(string? pokemonName, string? pokemonType)
        {
#if true
            if (!_isDataInDatabase) LoadFromApi(); //Loads the data from the API 
#endif
            PokemonService pokemonService = new(_dbContext, _configuration);
            IQueryable<Pokemon> query = pokemonService.GetDataFromDatabase();
			
            if (!string.IsNullOrEmpty(pokemonName))
				query = query.Where(p => p.Name.Contains(pokemonName));

			if (!string.IsNullOrEmpty(pokemonType))
				query = query.Where(p => p.Types.Any(t => t.Name.Contains(pokemonType)));

            ViewData["Pokemons"] = query.ToList();

			return View();
        }

		public void LoadFromApi()
		{
            PokemonService pokemonService = new(_dbContext, _configuration);

            List<Pokemon> pokemonList = pokemonService.GetDataFromApi();

            foreach (var pokemon in pokemonList)
            {
                foreach (var type in pokemon.Types.ToList())
                {
					var existingType = _dbContext.Types.Find(type.Id);
                    if (existingType != null)
                    {
                        pokemon.Types.Remove(type);
                        pokemon.Types.Add(existingType);
                    }
                }
            }

            _dbContext.Pokemons.AddRange(pokemonList);
            _dbContext.SaveChanges();

			_isDataInDatabase = true;
		}

		public IActionResult AddPokemon()
        {
            ViewData["Types"] = _dbContext.Types.ToList();
            return View();
        }

        [HttpPost]
		public IActionResult AddPokemon(Pokemon obj, int[] selectedTypes)
		{
            if (!ModelState.IsValid && selectedTypes.Length > 0)
            {
                obj.Types = _dbContext.Types.Where(t => selectedTypes.Contains(t.Id)).ToList();
                obj.PokemonSprite = new PokemonSprite()
                {
                    Id = 0,
                    BackDefault = null,
                    BackFemale = null,
                    BackShiny = null,
                    BackShinyFemale = null,
                    FrontDefault = null,
                    FrontFemale = null,
                    FrontShiny = null,
                    FrontShinyFemale = null,
                };
                PokemonService pokemonService = new(_dbContext, _configuration);
                pokemonService.AddPokemon(obj);
            }


            return RedirectToAction("Index");
		}

        public IActionResult UpdatePokemon(int? id)
        {
            if (id is null) return NotFound();
            Pokemon? pokemon = _dbContext.Pokemons.Find(id);
            if (pokemon is null) return NotFound();
            return View(pokemon);
        }

        [HttpPost]
		public IActionResult UpdatePokemon(Pokemon obj)
        {
            _dbContext.Pokemons.Update(obj);
            _dbContext.SaveChanges();

			return RedirectToAction("Index");
		}

        public IActionResult DeletePokemon(int? id)
        {
			if (id is null) return NotFound();
			Pokemon? pokemon = _dbContext.Pokemons.Include(p => p.PokemonSprite)
                                                  .Include(p => p.Types)
                                                  .Where(p => p.Id == id)
                                                  .FirstOrDefault();
			if (pokemon is null) return NotFound();
			return View(pokemon);
		}

		[HttpPost]
		public IActionResult DeletePokemon(Pokemon obj)
		{
			_dbContext.Pokemons.Remove(obj);
			_dbContext.SaveChanges();

			return RedirectToAction("Index");
		}
	}
}
