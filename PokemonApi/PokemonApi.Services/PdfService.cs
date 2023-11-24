using PokemonApi.Services.Interfaces;
using PokemonApi.Services.Pdf;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace PokemonApi.Services
{
    public class PdfService : IPdfService
    {
        public byte[] GetPdf(BasePdf pdfFile)
        {
            IDocument document = pdfFile.GenerateDocument();

            MemoryStream stream = new MemoryStream();

            document.GeneratePdf(stream);

            return stream.ToArray();
        }
    }
}
