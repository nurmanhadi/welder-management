using Customers.Client.Contracts;
using Customers.Client.Views;
using Customers.Core.Contracts;

namespace Customers.Infrastructure.Rpc;

public class CustomerRpcClient(ICustomerRepository customerRepository) : ICustomerClient
{
    public async Task<CustomerView?> GetByIdAsync(Guid id)
    {
        var customer = await customerRepository.GetByIdAsync(id);
        if (customer == null)
        {
            return null;
        }
        return new CustomerView(customer.Id, customer.Name, customer.Phone, customer.Address);
    }
}
