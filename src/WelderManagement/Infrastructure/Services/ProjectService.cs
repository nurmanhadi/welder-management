using System.Text.Json;
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
            CustomerId = customer.Id,
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
    public async Task<ProjectDetailDto> UpdateAsync(Guid id, UpdateProjectDto dto)
    {
        var project = await projectRepository.GetByIdAsync(id);
        if (project == null)
        {
            _logger.LogWarning("project {ProjectId} not found", id);
            throw new NotFoundException("project not found");
        }
        if (!string.IsNullOrEmpty(dto.Title))
        {
            project.Title = dto.Title;
        }
        if (!string.IsNullOrEmpty(dto.Description))
        {
            project.Description = dto.Description;
        }
        if (dto.Cost.HasValue)
        {
            if (project.Status == ProjectStatus.Approved && project.Status == ProjectStatus.InProgres && project.Status == ProjectStatus.Finished && project.Status == ProjectStatus.Cencelled)
            {
                _logger.LogWarning("cost cannot be updated because the project has already been {ProjectStatus}", project.Status);
                throw new ConflictException($"cost cannot be updated because the project has already been {project.Status}");
            }
            project.Cost = dto.Cost.Value;
        }
        if (dto.Status.HasValue)
        {
            if (!AllowedTransitionStatus[project.Status].Contains(dto.Status.Value))
            {
                _logger.LogWarning("Cannot change status from {currentStatus} to {newStatus}", project.Status, dto.Status.Value);
                throw new ConflictException($"Cannot change status from {project.Status} to {dto.Status.Value}");
            }
            project.Status = dto.Status.Value;
        }
        if (dto.StartDate.HasValue && dto.EndDate.HasValue)
        {
            switch (dto.Status)
            {
                case ProjectStatus.Approved:
                    if (!dto.StartDate.HasValue || !dto.EndDate.HasValue)
                    {
                        _logger.LogWarning("Project status {Status} requires both StartDate and EndDate.", dto.Status);
                        throw new ConflictException("start date and end date are required");
                    }
                    if (dto.EndDate < dto.StartDate)
                    {
                        _logger.LogWarning("EndDate {EndDate} must be greater than or equal to StartDate {StartDate}.", dto.EndDate, dto.StartDate);
                        throw new ConflictException("end date must be greater than or equal to start date");
                    }
                    project.StartDate = dto.StartDate.Value;
                    project.EndDate = dto.EndDate.Value;
                    break;
                case ProjectStatus.InProgres:
                    if (dto.StartDate.HasValue)
                    {
                        _logger.LogWarning("Attempt to update StartDate while project status is {Status}.", dto.Status);
                        throw new ConflictException("start date cannot be updated");
                    }
                    if (dto.EndDate.HasValue)
                    {
                        if (project.StartDate.HasValue && dto.EndDate < project.StartDate)
                        {
                            _logger.LogWarning("EndDate {EndDate} must be greater than or equal to StartDate {StartDate}.", dto.EndDate, project.StartDate);
                            throw new ConflictException("end date must be greater than or equal to start date");
                        }
                        project.EndDate = dto.EndDate.Value;
                    }
                    break;
                default:
                    if (dto.StartDate.HasValue || dto.EndDate.HasValue)
                    {
                        _logger.LogWarning("Project status {Status} cannot modify StartDate or EndDate.", dto.Status);
                        throw new ConflictException("date can only be set when project is approved or in progress");
                    }
                    break;
            }
        }
        await projectRepository.UpdateAsync(project);

        var customer = await customerClient.GetByIdAsync(project.CustomerId);
        if (customer == null)
        {
            _logger.LogWarning("customer {CustomerId} not found", project.CustomerId);
            throw new NotFoundException("customer not found");
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

    public async Task<ProjectDetailDto> GetByIdAsync(Guid id)
    {
        var project = await projectRepository.GetByIdAsync(id);
        if (project == null)
        {
            _logger.LogWarning("project {ProjectId} not found", id);
            throw new NotFoundException("project not found");
        }
        var customer = await customerClient.GetByIdAsync(project.CustomerId);
        if (customer == null)
        {
            _logger.LogWarning("customer {CustomerId} not found", project.CustomerId);
            throw new NotFoundException("customer not found");
        }
        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation("project {ProjectId} retrieved", project.Id);
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

    // other
    private static readonly Dictionary<ProjectStatus, ProjectStatus[]> AllowedTransitionStatus =
    new()
    {
        {ProjectStatus.Draft, [ProjectStatus.Survey, ProjectStatus.Cencelled]},
        {ProjectStatus.Survey, [ProjectStatus.Negotiation, ProjectStatus.Cencelled]},
        {ProjectStatus.Negotiation, [ProjectStatus.Approved, ProjectStatus.Cencelled]},
        {ProjectStatus.Approved, [ProjectStatus.InProgres]},
        {ProjectStatus.InProgres, [ProjectStatus.Finished]},
        {ProjectStatus.Finished, []},
        {ProjectStatus.Cencelled, []}
    };
}
