using System.Text.Json.Serialization;

namespace WelderManagement.Shared.Responses;

public record ApiResponse<T>(
    [property: JsonPropertyName("message")] string Message,
    [property: JsonPropertyName("data")] T? Data
);
