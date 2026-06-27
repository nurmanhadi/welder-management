using Customers.Client.Contracts;
using Microsoft.Extensions.Logging;
using Projects.Core.Commands;
using Projects.Core.Contracts;
using Projects.Core.Entities;
using Projects.Core.Helpers;
using Shared.Exceptions;

namespace Projects.Infrastructure.Services;

public class ProjectService(
    IProjectRepository projectRepository,
    ILogger<ProjectService> _logger,
    ICustomerClient customerClient

) : IProjectService
{
    public async Task<ProjectResult> AddDraftAsync(CreateProjectCommand command)
    {
        var customer = await customerClient.GetByIdAsync(command.CustomerId);
        if (customer == null)
        {
            _logger.LogWarning("customer {CustomerId} not found", command.CustomerId);
            throw new NotFoundException("customer not found");
        }
        var pid = PId.Generate();
        var project = new Project
        {
            PID = pid,
            CustomerId = command.CustomerId,
            Title = command.Title,
            Description = command.Description,
            Cost = command.Cost,
            Status = ProjectStatus.Draft
        };
        await projectRepository.AddAsync(project);
        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation("project {ProjectId} draft added successfully", project.Id);
        }
        return new ProjectResult(
            project.Id,
            project.PID,
            project.Title,
            project.Description,
            project.Cost,
            project.Status,
            project.StartDate,
            project.EndDate,
            new CustomerInfo(customer.Id,
                customer.Name,
                customer.Phone,
                customer.Address));
    }

    public Task<ProjectResult> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<ProjectResult> GetByPIDAsync(string pid)
    {
        throw new NotImplementedException();
    }

    public Task<ProjectResult> UpdateAsync(Guid id, UpdateProjectCommand command)
    {
        throw new NotImplementedException();
    }
}
