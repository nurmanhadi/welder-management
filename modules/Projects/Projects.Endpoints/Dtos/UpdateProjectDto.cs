using System.Text.Json.Serialization;
using Projects.Endpoints.Helpers;

namespace Projects.Endpoints.Dtos;

public record UpdateProjectDto(
    [property: JsonPropertyName("title")] string? Title,
    [property: JsonPropertyName("description")] string? Description,
    [property: JsonPropertyName("cost")] decimal? Cost,
    [property: JsonPropertyName("status")] Status? Status,
    [property: JsonPropertyName("start_date")] DateTime? StartDate,
    [property: JsonPropertyName("end_date")] DateTime? EndDate
);
