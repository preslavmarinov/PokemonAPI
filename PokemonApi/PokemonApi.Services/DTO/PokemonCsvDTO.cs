using PokemonApi.Data.Models.Identity;
using PokemonApi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonApi.Services.DTO
{
    public class PokemonCsvDTO
    {
        public string Name { get; set; }

        public string[] TypeIds { get; set; }

        public string[] Types { get; set; }

        public string HP { get; set; }

        public string Attack { get; set; }

        public string Defence { get; set; }

        public string Speed { get; set; }

        public string Generation { get; set; }

        public string Legendary { get; set; }

        public string LocationId { get; set; }

        public string Location { get; set; }

    }
}
