using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using PokedexApp.Data;
using PokedexApp.Models;
using System.Net.Http.Headers;

namespace PokedexApp.Service
{
    public class PokemonService : IService<Pokemon>
    {
        private readonly AppDbContext _context;
		private readonly HttpClient _client;

		public PokemonService(AppDbContext context) 
        {
            _context = context;
			_client = new HttpClient();
			_client.BaseAddress = new Uri("https://pokeapi.co/api/v2/");
			_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		}

        public List<Pokemon> GetDataFromApi()
        {
            TypeService typeService = new TypeService(_context);
			List<Models.Type> types = typeService.GetDataFromApi();

			List<Pokemon> pokemonList = new();
			HttpResponseMessage response = _client.GetAsync("pokemon?limit=60/").Result;
			if (response.IsSuccessStatusCode)
			{
				string responseString = response.Content.ReadAsStringAsync().Result;
				JObject jsonResponse = JObject.Parse(responseString);
				JArray results = (JArray)jsonResponse["results"];

				foreach (JToken result in results)
				{
					string pokemonName = result["name"].ToString();
					HttpResponseMessage resultsResponse = _client.GetAsync("pokemon/" + pokemonName).Result;
					if (response.IsSuccessStatusCode)
					{
						string detailData = resultsResponse.Content.ReadAsStringAsync().Result;
						JObject pokemonDetail = JObject.Parse(detailData);

						Pokemon pokemon = new()
						{
							Id = (int)pokemonDetail["id"],
							Name = char.ToUpper(((string)pokemonDetail["name"])[0]) + ((string)pokemonDetail["name"])[1..],
							Order = (int)pokemonDetail["order"],
                            Height = (int)pokemonDetail["height"],
                            Weight = (int)pokemonDetail["weight"],
                            IsDefault = (bool)pokemonDetail["is_default"],
                            BaseExperience= (int)pokemonDetail["base_experience"],
                            PokemonSprite = new PokemonSprite()
                            {
                                BackDefault = (string)pokemonDetail["sprites"]["back_default"],
                                BackFemale = (string)pokemonDetail["sprites"]["back_female"],
                                BackShiny = (string)pokemonDetail["sprites"]["back_shiny"],
                                BackShinyFemale = (string)pokemonDetail["sprites"]["back_shiny_female"],
								FrontDefault = (string)pokemonDetail["sprites"]["front_default"],
								FrontFemale = (string)pokemonDetail["sprites"]["front_female"],
								FrontShiny = (string)pokemonDetail["sprites"]["front_shiny"],
								FrontShinyFemale = (string)pokemonDetail["sprites"]["front_shiny_female"]
							}
						};
                        
                        foreach (var item in pokemonDetail["types"])
                            pokemon.Types.Add(types.Where(t => t.Name.ToLower() == (string)item["type"]["name"]).First());

						pokemonList.Add(pokemon);
					}
				}
			}

			return pokemonList;
		}

		public IQueryable<Pokemon> GetDataFromDatabase()
		{
			return _context.Pokemons.Include(p => p.PokemonSprite)
                                    .Include(p => p.Types);
		}

        public void AddPokemon(Pokemon pokemon)
        {
            Pokemon newPokemon = new()
            {
                Id = _context.Pokemons.ToList().Last().Id + 1,
                Name = pokemon.Name,
                Order = pokemon.Order,
                Height = pokemon.Height,
                Weight = pokemon.Weight,
                BaseExperience = pokemon.BaseExperience,
                IsDefault = pokemon.IsDefault,
                PokemonSprite = pokemon.PokemonSprite,
                Types = pokemon.Types
            };

            _context.Add(newPokemon);
            _context.SaveChanges();
        }

        public void UpdatePokemon(string pokemonName, Pokemon updated)
        {
            Pokemon pokemon = _context.Pokemons.Where(pokemon => pokemon.Name.Equals(pokemonName)).First();
            pokemon.Name = updated.Name;
            pokemon.Order = updated.Order;
            pokemon.Height = updated.Height;
            pokemon.Weight = updated.Weight;
            pokemon.BaseExperience = updated.BaseExperience;
            pokemon.IsDefault = updated.IsDefault;

            _context.Update(pokemon);
            _context.SaveChanges();
        }

        public void DeletePokemon(string pokemonName)
        {
            _context.Pokemons.Remove(_context.Pokemons.Where(pokemon => pokemon.Name == pokemonName).First());
            _context.SaveChanges();
        }

	}
}
