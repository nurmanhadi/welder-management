using WelderManagement.Applications.Dtos;
using WelderManagement.Domain.Contracts.Clients;
using WelderManagement.Domain.Contracts.Repositories;
using WelderManagement.Domain.Contracts.Services;
using WelderManagement.Domain.Entities;
using WelderManagement.Domain.Enums;
using WelderManagement.Shared.Exceptions;
using WelderManagement.Shared.Tools;

namespace WelderManagement.Infrastructure.Services;

public class ProjectService(
    IProjectRepository projectRepository,
    ILogger<ProjectService> _logger,
    ICustomerClient customerClient

) : IProjectService
{
    public async Task<ProjectDetailDto> CreateDraftAsync(CreateDraftProjectDto dto)
    {
        var customer = await customerClient.GetByIdAsync(dto.CustomerId);
        if (customer == null)
        {
            _logger.LogWarning("customer {CustomerId} not found", dto.CustomerId);
            throw new NotFoundException("customer not found");
        }
        var pid = PId.Generate();
        var project = new Project
        {
            PID = pid,
            CustomerId = dto.CustomerId,
            Title = dto.Title,
            Description = dto.Description,
            Cost = dto.Cost,
            Status = ProjectStatus.Draft
        };
        await projectRepository.InsertAsync(project);
        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation("project {ProjectId} draft added successfully", project.Id);
        }
        return new ProjectDetailDto(
            project.Id,
            project.PID,
            project.Title,
            project.Description,
            project.Cost,
            project.Status,
            project.StartDate,
            project.EndDate,
            new CustomerSummaryDto(
                customer.Id,
                customer.Name
            )
        );
    }
    public Task<ProjectDetailDto> UpdateAsync(Guid id, UpdateProjectDto dto)
    {
        throw new NotImplementedException();
    }

    Task<ProjectDetailDto> IProjectService.GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    Task<ProjectDetailDto> IProjectService.GetByPIDAsync(string pid)
    {
        throw new NotImplementedException();
    }
}
