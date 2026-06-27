using System.Text.Json.Serialization;

namespace Projects.Endpoints.Dtos;

public record CustomerResponse(
    [property: JsonPropertyName("id")] Guid Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("phone")] string Phone,
    [property: JsonPropertyName("address")] string Address);
