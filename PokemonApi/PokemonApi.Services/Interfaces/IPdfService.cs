using PokemonApi.Services.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonApi.Services.Interfaces
{
    public interface IPdfService
    {
        byte[] GetPdf(BasePdf pdfFile);
    }
}
