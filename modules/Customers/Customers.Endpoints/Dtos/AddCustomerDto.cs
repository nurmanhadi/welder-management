using System.Text.Json.Serialization;

namespace Customers.Endpoints.Dtos;

public record AddCustomerDto(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("phone")] string Phone,
    [property: JsonPropertyName("address")] string Address
);
