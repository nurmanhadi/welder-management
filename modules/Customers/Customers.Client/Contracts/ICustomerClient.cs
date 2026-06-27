using Customers.Client.Views;

namespace Customers.Client.Contracts;

public interface ICustomerClient
{
    Task<CustomerView> GetByIdAsync(Guid id);
}
