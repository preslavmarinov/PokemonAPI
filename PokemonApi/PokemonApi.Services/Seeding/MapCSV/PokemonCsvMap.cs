﻿using CsvHelper.Configuration;
using PokemonApi.Services.Seeding.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonApi.Services.Seeding.MapCSV
{
    public class PokemonCsvMap : ClassMap<PokemonCsvDTO>
    {
        public PokemonCsvMap()
        {
            Map(m => m.Name).Name("name");

            Map(m => m.TypeIds).Convert(row => row.Row.GetField("type_ids")!
            .Trim(new char[] { '[', ']' })
            .Split(", ")
            .ToArray()
            );

            Map(m => m.Types).Convert(row => row.Row.GetField("types")!
            .Trim(new char[] { '[', ']' })
            .Split(", ")
            .ToArray()
            );

            Map(m => m.HP).Name("hp");

            Map(m => m.Attack).Name("attack");

            Map(m => m.Defence).Name("defence");

            Map(m => m.Speed).Name("speed");

            Map(m => m.Legendary).Name("legendary");

            Map(m => m.Generation).Name("generation");

            Map(m => m.LocationId).Name("location_id");

            Map(m => m.Location).Name("location");

        }
    }
}
