using CsvHelper;
using Microsoft.EntityFrameworkCore;
using PokemonApi.Data;
using PokemonApi.Data.Models;
using PokemonApi.Services.Interfaces;
using PokemonApi.Services.Seeding.DTO;
using PokemonApi.Services.Seeding.MapCSV;
using System.Collections.Generic;
using System.Globalization;

namespace PokemonApi.Services.Seeding
{
    public class Seeder : ISeeder
    {
        private readonly PokemonDbContext _context;
        private readonly Dictionary<string, TypeEntity> Types = new Dictionary<string, TypeEntity>();
        private readonly Dictionary<string, LocationEntity> Locations = new Dictionary<string, LocationEntity>();

        public Seeder(PokemonDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync(string path)
        {
            if(await this._context.Pokemons.AnyAsync())
            {
                return;
            }

            using var reader = new StreamReader(path);
            using var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);

            csvReader.Context.RegisterClassMap<PokemonCsvMap>();
            while(csvReader.Read())
            {
                var item = csvReader.GetRecord<PokemonCsvDTO>();

                if(!this.Locations.ContainsKey(item.LocationId))
                {
                    this.Locations[item.LocationId] = new LocationEntity
                    {
                        Name = item.Location,
                    };
                }

                for(int j=0; j<item.TypeIds.Length; j++)
                {
                    string typeName = item.Types[j];
                    string typeId = item.TypeIds[j];

                    if (!this.Types.ContainsKey(typeId))
                    {
                        this.Types[typeId] = new TypeEntity
                        {
                            Name = typeName
                        };
                    }
                }

                this._context.Add(new PokemonEntity
                {
                    Name = item.Name,
                    HP = int.Parse(item.HP),
                    Attack = int.Parse(item.Attack),
                    Defence = int.Parse(item.Defence),
                    Speed = int.Parse(item.Speed),
                    Generation = int.Parse(item.Generation),
                    IsLegendary = bool.Parse(item.Legendary),
                    Types = this.Types.Where(x => item.TypeIds.Contains(x.Key))
                    .Select(x => new PokemonType { Type = x.Value })
                    .ToList(),
                    Location = this.Locations[item.LocationId]
                });
                
            }

            await _context.SaveChangesAsync();
        }
    }
}