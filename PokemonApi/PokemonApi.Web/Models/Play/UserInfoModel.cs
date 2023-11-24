using PokemonApi.Data.Models.Identity;
using PokemonApi.Web.Models.Pokemon;

namespace PokemonApi.Web.Models.Play
{
    public class UserInfoModel
    {
        public ApplicationUser User { get; set; }

        public IEnumerable<PokemonViewModel> Pokemons { get; set; } = default!;
    }
}
