using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PokedexApp.Models
{
    public class Type
    {
		public int Id { get; set; }
        public string Name { get; set; } = null!;
        public List<Pokemon> Pokemons { get; set; } = new();
    }
}
