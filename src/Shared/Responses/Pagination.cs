using System.Text.Json.Serialization;

namespace Shared.Responses;

public record Pagination<T>(
    [property: JsonPropertyName("contents")] IReadOnlyCollection<T> Contents,
    [property: JsonPropertyName("page")] int Page,
    [property: JsonPropertyName("page_size")] int PageSize,
    [property: JsonPropertyName("total_items")] int TotalItems
)
{
    [JsonPropertyName("total_pages")]
    public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
}
