namespace PokemonApi.Web.Models.Pokemon
{
    public class PokemonInputModel
    {
        public string Name { get; set; }

        public string HP { get; set; }

        public string Attack { get; set; }

        public string Defence { get; set; }

        public string Speed { get; set; }

        public string Generation { get; set; }

        public string IsLegendary { get; set; }

        public Guid[] TypeIds { get; set; }

        public Guid LocationId { get; set; }
    }
}
