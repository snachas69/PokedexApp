using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PokedexApp.Models
{
    public class Pokemon
    {
		public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        [Required]
		public int BaseExperience { get; set; }
        [Required]
        public int Height { get; set; }
        [Required]
        public bool IsDefault { get; set; }
        [Required]
        public int Order { get; set; }
        [Required]
        public int Weight { get; set; }
        public PokemonSprite PokemonSprite { get; set; } = null!;
        public List<Type> Types { get; set; } = new();
    }
}
