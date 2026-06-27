using Projects.Core.Helpers;

namespace Projects.Core.Commands;

public record CreateProjectCommand(
    Guid CustomerId,
    string Title,
    string Description,
    decimal Cost,
    ProjectStatus ProjectStatus,
    DateTime StartDate,
    DateTime EndDate
);
