using Projects.Core.Commands;

namespace Projects.Core.Contracts;

public interface IProjectService
{
    Task<ProjectResult> GetByIdAsync(Guid id);
    Task<ProjectResult> GetByPIDAsync(string pid);
    Task<ProjectResult> AddAsync(CreateProjectCommand command);
    Task<ProjectResult> UpdateAsync(Guid id, UpdateProjectCommand command);
}
