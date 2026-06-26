using Customers.Core.Entities;
using Customers.Core.Helpers;
using Shared.Abstractions;
using Shared.Responses;

namespace Customers.Core.Contracts;

public interface ICustomerRepository : IRepository<Customer>
{
    Task<int> CountByPhoneAsync(string phone);
    Task<Pagination<Customer>> GetAllAsync(CustomerFilter filter);
}
