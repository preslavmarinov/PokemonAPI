using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PokemonApi.Data;
using PokemonApi.Data.Models;
using PokemonApi.Data.Models.Identity;
using PokemonApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonApi.Services
{
    public class PlayService : IPlayService
    {
        private readonly PokemonDbContext _pokemonContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public PlayService(PokemonDbContext pokemonContext, UserManager<ApplicationUser> userManager)
        {
            _pokemonContext = pokemonContext;
            _userManager = userManager;
        }

        public async Task<string> BattlePokemon(Guid locationId,int index, Guid userId)
        {
            var wildPokemons = await this._pokemonContext.Pokemons.Where(x => x.LocationId == locationId && x.ApplicationUserId == null).ToListAsync();
            Random rand = new Random();
            int randomNumber = rand.Next(0, wildPokemons.Count);
            var pokemonToBeBattled = wildPokemons[randomNumber];

            var myPokemons = await this._pokemonContext.Pokemons.Where(x => x.ApplicationUserId == userId).ToListAsync();
            if(index > myPokemons.Count)
            {
                throw new InvalidOperationException("You don't have that many Pokemons");
            }
            var chosenPokemon = myPokemons[index-1];

            int i = 0;
            while(chosenPokemon.HP > 0 && pokemonToBeBattled.HP > 0)
            {
                if(i%2 == 0)
                {
                    if(pokemonToBeBattled.Defence > 0)
                    {
                        pokemonToBeBattled.Defence -= (chosenPokemon.Attack + chosenPokemon.Speed) / 2;
                    }
                    else
                    {
                        pokemonToBeBattled.HP -= (chosenPokemon.Attack + chosenPokemon.Speed) / 2;
                    }
                }
                else
                {
                    if (chosenPokemon.Defence > 0)
                    {
                        chosenPokemon.Defence -= (pokemonToBeBattled.Attack + pokemonToBeBattled.Speed) / 2;
                    }
                    else
                    {
                        chosenPokemon.HP -= (pokemonToBeBattled.Attack + pokemonToBeBattled.Speed) / 2;
                    }
                }
                i++;
            }

            if (chosenPokemon.HP > 0) return "Battle Won";
            else return "Battle Lost";
        }

        public async Task<PokemonEntity> CatchPokemon(Guid locationId, Guid userId)
        {
            var wildPokemons = await this._pokemonContext.Pokemons.Where(x => x.LocationId == locationId && x.ApplicationUserId == null).ToListAsync();
            Random rand = new Random();
            int randomNumber = rand.Next(0, wildPokemons.Count);
            var pokemonToBeCaught = wildPokemons[randomNumber];

            double complexity = pokemonToBeCaught.HP + pokemonToBeCaught.Attack + pokemonToBeCaught.Defence + pokemonToBeCaught.Speed;
            if (pokemonToBeCaught.IsLegendary) complexity *= 1.5;

            double upperLimit = 0.85;
            double catchComplexity = rand.NextDouble() * upperLimit;

            if (catchComplexity > complexity / 1000)
            {
                pokemonToBeCaught.ApplicationUserId = userId;
                this._pokemonContext.Pokemons.Update(pokemonToBeCaught);

                await this._pokemonContext.SaveChangesAsync();

                return pokemonToBeCaught;
            }
            else return null;
        }
    }
}
