using System.Text.Json.Serialization;

namespace dotnet_ef_rpg.Models;

[JsonConverter(typeof(JsonStringEnumConverter))] // converts enum to and from strings, so that swagger puts the names and not the numbers in the schemas
public enum RpgClass
{
    Knight = 1,
    Mage = 2,
    Cleric = 3
}