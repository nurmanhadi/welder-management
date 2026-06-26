using Projects.Core.Helpers;

namespace Projects.Core.Commands;

public record UpdateProjectCommand(
    string Title,
    string Description,
    decimal Cost,
    ProjectStatus ProjectStatus,
    DateTime StartDate,
    DateTime EndDate
);
