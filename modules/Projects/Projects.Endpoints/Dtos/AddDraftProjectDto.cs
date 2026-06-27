using System.Text.Json.Serialization;

namespace Projects.Endpoints.Dtos;

public record AddDraftProjectDto(
    [property: JsonPropertyName("customer_id")] Guid CustomerId,
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("description")] string Description,
    [property: JsonPropertyName("cost")] decimal Cost
);
