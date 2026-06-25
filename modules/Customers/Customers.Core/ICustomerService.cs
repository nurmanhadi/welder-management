using Shared.Responses;

namespace Customers.Core;

public interface ICustomerService
{
    Task<ResponseCustomerCommand> GetByIdAsync(Guid id);
    Task<ResponseCustomerCommand> AddAsync(AddCustomerCommand command);
    Task<ResponseCustomerCommand> UpdateAsync(UpdateCustomerCommand command);
    Task DeleteAsync(Guid id);
    Task<Pagination<ResponseCustomerCommand>> GetAllAsync(CustomerFilter filter);
}
