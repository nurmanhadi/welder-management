using Projects.Core.Helpers;

namespace Projects.Core.Commands;

public record CreateProjectCommand(
    string CustomerId,
    string Title,
    string Description,
    decimal Cost,
    ProjectStatus ProjectStatus,
    DateTime StartDate,
    DateTime EndDate
);
