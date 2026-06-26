using System.Text.Json.Serialization;

namespace Customers.Endpoints.Helpers;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Sort
{
    Name = 1,
    Phone = 2,
    CreatedAt = 3,
}
