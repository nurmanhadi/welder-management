using Customers.Core.Commands;
using Customers.Core.Helpers;
using Shared.Responses;

namespace Customers.Core.Contracts;

public interface ICustomerService
{
    Task<CustomerResult> GetByIdAsync(Guid id);
    Task<CustomerResult> AddAsync(AddCustomerCommand command);
    Task<CustomerResult> UpdateAsync(UpdateCustomerCommand command);
    Task DeleteAsync(Guid id);
    Task<Pagination<CustomerResult>> GetAllAsync(CustomerFilter filter);
}
