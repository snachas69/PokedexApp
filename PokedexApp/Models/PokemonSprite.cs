using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PokedexApp.Models
{
    public class PokemonSprite
    {
        [Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
        public string? FrontDefault { get; set; }
        public string? FrontShiny { get; set; }
        public string? FrontFemale { get; set; }
        public string? FrontShinyFemale { get; set; }
        public string? BackDefault { get; set; }
        public string? BackShiny { get; set; }
        public string? BackFemale { get; set; }
        public string? BackShinyFemale { get; set; }
        public List<Pokemon> Pokemons { get; set; } = new();
    }
}