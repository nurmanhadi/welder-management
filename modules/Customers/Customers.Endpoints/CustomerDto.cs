using System.Text.Json.Serialization;

namespace Customers.Endpoints;

public record AddCustomerDto(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("phone")] string Phone,
    [property: JsonPropertyName("address")] string Address);
public record UpdateCustomerDto(
    [property: JsonPropertyName("name")] string? Name,
    [property: JsonPropertyName("phone")] string? Phone,
    [property: JsonPropertyName("address")] string? Address);
public record ResponseCustomerDto(
    [property: JsonPropertyName("id")] Guid Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("phone")] string Phone,
    [property: JsonPropertyName("address")] string Address);
