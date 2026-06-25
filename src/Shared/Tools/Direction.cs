using System.Text.Json.Serialization;

namespace Shared.Tools;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Direction
{
    Ascending = 1,
    Descending = -1,
}
