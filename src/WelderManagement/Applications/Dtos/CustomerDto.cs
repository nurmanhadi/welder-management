using System.Text.Json.Serialization;

namespace WelderManagement.Applications.Dtos;

public record CreateCustomerDto(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("phone")] string Phone,
    [property: JsonPropertyName("address")] string Address
);
public record UpdateCustomerDto(
    [property: JsonPropertyName("name")] string? Name,
    [property: JsonPropertyName("phone")] string? Phone,
    [property: JsonPropertyName("address")] string? Address
);
public record CustomerDetailDto(
    [property: JsonPropertyName("id")] Guid Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("phone")] string Phone,
    [property: JsonPropertyName("address")] string Address
);
public record CustomerSummaryDto(
    [property: JsonPropertyName("id")] Guid Id,
    [property: JsonPropertyName("name")] string Name
);
