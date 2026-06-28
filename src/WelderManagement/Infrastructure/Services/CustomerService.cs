using WelderManagement.Applications.Dtos;
using WelderManagement.Domain.Contracts.Repositories;
using WelderManagement.Domain.Contracts.Services;
using WelderManagement.Domain.Entities;
using WelderManagement.Shared.Exceptions;
using WelderManagement.Shared.Responses;
using WelderManagement.Shared.Sorts;
using WelderManagement.Shared.Tools;

namespace WelderManagement.Infrastructure.Services;

public class CustomerService(ICustomerRepository customerRepository, ILogger<CustomerService> _logger) : ICustomerService
{
    public async Task<CustomerDetailDto> CreateAsync(CreateCustomerDto dto)
    {
        await PhoneExistsAsync(dto.Phone);
        var customer = new Customer
        {
            Name = dto.Name,
            Phone = dto.Phone,
            Address = dto.Address
        };
        await customerRepository.InsertAsync(customer);
        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation("customer {Name} added successfully", customer.Name);
        }
        return new CustomerDetailDto(customer.Id, customer.Name, customer.Phone, customer.Address);
    }

    public async Task DeleteAsync(Guid id)
    {
        await IdExistsAsync(id);
        await customerRepository.DeleteAsync(id);
        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation("customer {Id} deleted successfully", id);
        }
    }

    public async Task<CustomerDetailDto> GetByIdAsync(Guid id)
    {
        var customer = await customerRepository.GetByIdAsync(id);
        if (customer == null)
        {
            _logger.LogWarning("customer {Id} not found", id);
            throw new NotFoundException("Customer not found");
        }
        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation("customer {Id} retrieved successfully", id);
        }
        return new CustomerDetailDto(customer.Id, customer.Name, customer.Phone, customer.Address);
    }

    public async Task<CustomerDetailDto> UpdateAsync(Guid id, UpdateCustomerDto dto)
    {
        var customer = await customerRepository.GetByIdAsync(id);
        if (customer == null)
        {
            _logger.LogWarning("customer {Id} not found", id);
            throw new NotFoundException("Customer not found");
        }
        if (!string.IsNullOrEmpty(dto.Name))
        {
            customer.Name = dto.Name;
        }
        if (!string.IsNullOrEmpty(dto.Phone))
        {
            await PhoneExistsAsync(dto.Phone);
            customer.Phone = dto.Phone;
        }
        if (!string.IsNullOrEmpty(dto.Address))
        {
            customer.Address = dto.Address;
        }
        await customerRepository.UpdateAsync(customer);
        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation("customer {Id} updated successfully", id);
        }
        return new CustomerDetailDto(customer.Id, customer.Name, customer.Phone, customer.Address);
    }

    public async Task<Pagination<CustomerSummaryDto>> GetAllAsync(int page, int pageSize, CustomerSort sort, Direction direction, string? search)
    {
        var result = await customerRepository.GetAllAsync(page, pageSize, sort, direction, search);
        var contents = result.Contents.Select(x => new CustomerSummaryDto(x.Id, x.Name)).ToList().AsReadOnly();
        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation("customer count {Count} retrieved successfully", contents.Count);
        }
        return new Pagination<CustomerSummaryDto>(contents, result.Page, result.PageSize, result.TotalItems);
    }

    // other
    private async Task PhoneExistsAsync(string phone)
    {
        var count = await customerRepository.CountByPhoneAsync(phone);
        if (count > 0)
        {
            _logger.LogWarning("phone {Phone} already exists", phone);
            throw new ConflictException("Phone already exists");
        }
    }
    private async Task IdExistsAsync(Guid id)
    {
        var count = await customerRepository.CountByIdAsync(id);
        if (count < 1)
        {
            _logger.LogWarning("customer {Id} not found", id);
            throw new NotFoundException("Customer not found");
        }
    }
}
