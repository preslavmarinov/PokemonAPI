using PokemonApi.Web.Models.Location;
using PokemonApi.Web.Models.Type;

namespace PokemonApi.Web.Models.Pokemon
{
    public class PokemonViewModel
    {
        public string Name { get; set; }

        public string HP { get; set; }

        public string Attack { get; set; }

        public string Defence { get; set; }

        public string Speed { get; set; }

        public string Generation { get; set; }

        public string IsLegendary { get; set; }

        public TypeViewInputModel[] Types { get; set; }

        public LocationViewInputModel Location { get; set; }

        public string? Owner { get; set; }
    }
}
