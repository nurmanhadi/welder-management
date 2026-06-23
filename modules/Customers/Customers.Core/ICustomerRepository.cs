using Shared.Abstractions;

namespace Customers.Core;

public interface ICustomerRepository : IRepository<Customer>
{
    Task<int> CountByPhoneAsync(string phone);
}
