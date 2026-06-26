using Projects.Core.Helpers;

namespace Projects.Core.Commands;

public record ProjectResult(
    Guid Id,
    string PID,
    string Title,
    string Description,
    decimal Cost,
    ProjectStatus ProjectStatus,
    DateTime StartDate,
    DateTime EndDate,
    CustomerInfo CustomerInfo
);
