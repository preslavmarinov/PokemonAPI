
namespace PokemonApi.Services.Interfaces
{
    public interface ISeeder
    {
        public Task SeedAsync(string path);
    }
}
