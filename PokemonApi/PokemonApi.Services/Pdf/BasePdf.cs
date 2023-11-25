using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonApi.Services.Pdf
{
    public abstract class BasePdf
    {
        public abstract IDocument CreateDocument();
    }
}
