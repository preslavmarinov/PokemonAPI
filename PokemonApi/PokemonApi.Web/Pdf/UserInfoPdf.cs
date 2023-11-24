using PokemonApi.Services.Pdf;
using PokemonApi.Web.Models.Play;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Text.RegularExpressions;

namespace PokemonApi.Web.Pdf
{
    public class UserInfoPdf : BasePdf
    {
        private const string HEADING = "User Info";
        private readonly UserInfoModel model;

        public UserInfoPdf(UserInfoModel model)
        {
            this.model = model;
        }

        public override IDocument GenerateDocument()
            => Document.Create(doc =>
            {
                doc.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.MarginVertical(24);
                    page.MarginHorizontal(8);

                    page.Content()
                        .Column(col =>
                        {
                            col.Item()
                                .AlignCenter()
                                .DefaultTextStyle((text)
                                    => text.Bold().FontColor("#000000").FontSize(32))
                                .Text(HEADING);

                            col.Item()
                                .PaddingTop(16)
                                .AlignCenter()
                                .DefaultTextStyle((text)
                                    => text.Bold().FontColor("#000000").FontSize(24))
                                .Text(model.User.UserName);

                            foreach (var pokemon in model.Pokemons)
                            {
                                col.Item()
                               .PaddingHorizontal(32)
                               .PaddingVertical(16)
                               .Table(t =>
                               {
                                   t.ColumnsDefinition(c =>
                                   {
                                       c.RelativeColumn();
                                       c.RelativeColumn();
                                       c.RelativeColumn();
                                       c.RelativeColumn();
                                       c.RelativeColumn();
                                       c.RelativeColumn();
                                       c.RelativeColumn(2);
                                   });

                                   t.Header(th =>
                                   {
                                       th.Cell()
                                         .ColumnSpan(7)
                                         .Background(Colors.Grey.Medium)
                                         .Border(1)
                                         .AlignMiddle()
                                         .AlignCenter()
                                         .Text(pokemon.Name)
                                         .Bold();
                                       th.Cell()
                                         .ColumnSpan(1)
                                         .Background(Colors.Grey.Lighten2)
                                         .Border(1)
                                         .AlignMiddle()
                                         .AlignCenter()
                                         .Text("HP")
                                         .Bold();
                                       th.Cell()
                                         .ColumnSpan(1)
                                         .Background(Colors.Grey.Lighten2)
                                         .Border(1)
                                         .AlignMiddle()
                                         .AlignCenter()
                                         .Text("Attack")
                                         .Bold();
                                       th.Cell()
                                         .ColumnSpan(1)
                                         .Background(Colors.Grey.Lighten2)
                                         .Border(1)
                                         .AlignMiddle()
                                         .AlignCenter()
                                         .Text("Defence")
                                         .Bold();
                                       th.Cell()
                                         .ColumnSpan(1)
                                         .Background(Colors.Grey.Lighten2)
                                         .Border(1)
                                         .AlignMiddle()
                                         .AlignCenter()
                                         .Text("Speed")
                                         .Bold();
                                       th.Cell()
                                        .ColumnSpan(1)
                                        .Background(Colors.Grey.Lighten2)
                                        .Border(1)
                                        .AlignMiddle()
                                        .AlignCenter()
                                        .Text("Legendary")
                                        .Bold();
                                       th.Cell()
                                         .ColumnSpan(1)
                                         .Background(Colors.Grey.Lighten2)
                                         .Border(1)
                                         .AlignMiddle()
                                         .AlignCenter()
                                         .Text("Location")
                                         .Bold();
                                       th.Cell()
                                         .ColumnSpan(1)
                                         .Background(Colors.Grey.Lighten2)
                                         .Border(1)
                                         .AlignMiddle()
                                         .AlignCenter()
                                         .Text("Types")
                                         .Bold();
                                   });

                                   t.Cell().Border(1).AlignMiddle().AlignCenter().Text(pokemon.HP);
                                   t.Cell().Border(1).AlignMiddle().AlignCenter().Text(pokemon.Attack);
                                   t.Cell().Border(1).AlignMiddle().AlignCenter().Text(pokemon.Defence);
                                   t.Cell().Border(1).AlignMiddle().AlignCenter().Text(pokemon.Speed);
                                   t.Cell().Border(1).AlignMiddle().AlignCenter().Text(pokemon.IsLegendary);
                                   t.Cell().Border(1).AlignMiddle().AlignCenter().Text(pokemon.Location.Name);

                                   string pokemonTypes = "";
                                   for(int i = 0; i < pokemon.Types.Length; i++)
                                   {
                                       if (i == pokemon.Types.Length - 1) pokemonTypes += pokemon.Types[i].Name;
                                       else pokemonTypes += pokemon.Types[i].Name + ", ";
                                   }
                                   t.Cell().Border(1).AlignMiddle().AlignCenter().Text(pokemonTypes);
                               });
                            }
                        });
                });
            });
    }
}
