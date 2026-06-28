using WelderManagement.Applications.Dtos;
using WelderManagement.Shared.Responses;
using WelderManagement.Shared.Sorts;
using WelderManagement.Shared.Tools;

namespace WelderManagement.Domain.Contracts.Services;

public interface ICustomerService
{
    Task<CustomerDetailDto> GetByIdAsync(Guid id);
    Task<CustomerDetailDto> CreateAsync(CreateCustomerDto dto);
    Task<CustomerDetailDto> UpdateAsync(Guid id, UpdateCustomerDto dto);
    Task DeleteAsync(Guid id);
    Task<Pagination<CustomerSummaryDto>> GetAllAsync(int page, int pageSize, CustomerSort sort, Direction direction, string? search);
}
