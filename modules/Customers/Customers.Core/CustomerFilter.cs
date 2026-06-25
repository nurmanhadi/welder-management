using Shared.Tools;

namespace Customers.Core;

public record CustomerFilter(
    int Page,
    int PageSize,
    CustomerSort Sort,
    Direction Direction,
    string? Search = null);

public enum CustomerSort
{
    Name = 1,
    Phone = 2,
    CreatedAt = 3,
}
