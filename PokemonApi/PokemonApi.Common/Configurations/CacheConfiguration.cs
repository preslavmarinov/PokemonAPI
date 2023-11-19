using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonApi.Common.Configurations
{
    public class CacheConfiguration
    {
        public long MaxSize { get; set; }

        public long Duration { get; set; }
    }
}
