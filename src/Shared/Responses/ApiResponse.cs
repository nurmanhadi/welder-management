using System.Text.Json.Serialization;

namespace Shared.Responses;

public record ApiResponse<T>(
    [property: JsonPropertyName("message")] string Message,
    [property: JsonPropertyName("data")] T? Data
);
