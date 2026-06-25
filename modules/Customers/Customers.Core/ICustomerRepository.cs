using Shared.Abstractions;
using Shared.Responses;

namespace Customers.Core;

public interface ICustomerRepository : IRepository<Customer>
{
    Task<int> CountByPhoneAsync(string phone);
    Task<Pagination<Customer>> GetAllAsync(CustomerFilter filter);
}
