using Microsoft.Extensions.Logging;
using Shared.Exceptions;
using Shared.Responses;

namespace Customers.Core;

public class CustomerService(ICustomerRepository _db, ILogger<CustomerService> _logger) : ICustomerService
{
    public async Task<ResponseCustomerCommand> AddAsync(AddCustomerCommand command)
    {
        await PhoneExistsAsync(command.Phone);
        var customer = new Customer
        {
            Name = command.Name,
            Phone = command.Phone,
            Address = command.Address
        };
        await _db.AddAsync(customer);
        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation("customer {Name} added successfully", customer.Name);
        }
        return new ResponseCustomerCommand(customer.Id, customer.Name, customer.Phone, customer.Address);
    }

    public async Task DeleteAsync(Guid id)
    {
        await IdExistsAsync(id);
        await _db.DeleteAsync(id);
        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation("customer {Id} deleted successfully", id);
        }
    }

    public async Task<ResponseCustomerCommand> GetByIdAsync(Guid id)
    {
        var customer = await _db.GetByIdAsync(id);
        if (customer == null)
        {
            _logger.LogWarning("customer {Id} not found", id);
            throw new NotFoundException("Customer not found");
        }
        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation("customer {Id} retrieved successfully", id);
        }
        return new ResponseCustomerCommand(customer.Id, customer.Name, customer.Phone, customer.Address);
    }

    public async Task<ResponseCustomerCommand> UpdateAsync(UpdateCustomerCommand command)
    {
        var customer = await _db.GetByIdAsync(command.Id);
        if (customer == null)
        {
            _logger.LogWarning("customer {Id} not found", command.Id);
            throw new NotFoundException("Customer not found");
        }
        if (!string.IsNullOrEmpty(command.Name))
        {
            customer.Name = command.Name;
        }
        if (!string.IsNullOrEmpty(command.Phone))
        {
            await PhoneExistsAsync(command.Phone);
            customer.Phone = command.Phone;
        }
        if (!string.IsNullOrEmpty(command.Address))
        {
            customer.Address = command.Address;
        }
        await _db.UpdateAsync(customer);
        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation("customer {Id} updated successfully", command.Id);
        }
        return new ResponseCustomerCommand(customer.Id, customer.Name, customer.Phone, customer.Address);
    }

    private async Task PhoneExistsAsync(string phone)
    {
        var count = await _db.CountByPhoneAsync(phone);
        if (count > 0)
        {
            _logger.LogWarning("phone {Phone} already exists", phone);
            throw new ConflictException("Phone already exists");
        }
    }
    private async Task IdExistsAsync(Guid id)
    {
        var count = await _db.CountByIdAsync(id);
        if (count < 1)
        {
            _logger.LogWarning("customer {Id} not found", id);
            throw new NotFoundException("Customer not found");
        }
    }

    public async Task<Pagination<ResponseCustomerCommand>> GetAllAsync(CustomerFilter filter)
    {
        var result = await _db.GetAllAsync(filter);
        var contents = result.Contents.Select(x => new ResponseCustomerCommand(x.Id, x.Name, x.Phone, x.Address)).ToList().AsReadOnly();
        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation("customer count {Count} retrieved successfully", contents.Count);
        }
        return new Pagination<ResponseCustomerCommand>(contents, result.Page, result.PageSize, result.TotalItems);
    }
}
