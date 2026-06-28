using WelderManagement.Applications.Dtos;

namespace WelderManagement.Domain.Contracts.Services;

public interface IProjectService
{
    Task<ProjectDetailDto> GetByIdAsync(Guid id);
    Task<ProjectDetailDto> GetByPIDAsync(string pid);
    Task<ProjectDetailDto> CreateDraftAsync(CreateDraftProjectDto dto);
    Task<ProjectDetailDto> UpdateAsync(Guid id, UpdateProjectDto dto);
}
