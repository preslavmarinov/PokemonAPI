
using System.Text.Json.Serialization;

namespace PokemonApi.Common.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SortAttribute
    {
        Name,
        HP,
        Attack,
        Defence,
        Speed,
        Generation,
        IsLegendary,
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SortDirection
    {
        ASC,
        DESC,
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Role
    {
        user,
        admin
    }
}
