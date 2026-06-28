using System.Text.Json.Serialization;
using WelderManagement.Domain.Enums;

namespace WelderManagement.Applications.Dtos;

public record CreateDraftProjectDto(
    [property: JsonPropertyName("customer_id")] Guid CustomerId,
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("description")] string Description,
    [property: JsonPropertyName("cost")] decimal Cost
);
public record UpdateProjectDto(
    [property: JsonPropertyName("title")] string? Title,
    [property: JsonPropertyName("description")] string? Description,
    [property: JsonPropertyName("cost")] decimal? Cost,
    [property: JsonPropertyName("status")] ProjectStatus? Status,
    [property: JsonPropertyName("start_date")] DateTime? StartDate,
    [property: JsonPropertyName("end_date")] DateTime? EndDate
);
public record ProjectDetailDto(
    [property: JsonPropertyName("id")] Guid Id,
    [property: JsonPropertyName("pid")] string PId,
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("description")] string Description,
    [property: JsonPropertyName("cost")] decimal Cost,
    [property: JsonPropertyName("status")] ProjectStatus Status,
    [property: JsonPropertyName("start_date")] DateTime? StartDate,
    [property: JsonPropertyName("end_date")] DateTime? EndDate,
    [property: JsonPropertyName("customer")] CustomerSummaryDto Customer
);
