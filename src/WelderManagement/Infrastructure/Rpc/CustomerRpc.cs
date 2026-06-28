using WelderManagement.Applications.Queries;
using WelderManagement.Domain.Contracts.Clients;
using WelderManagement.Domain.Contracts.Repositories;

namespace WelderManagement.Infrastructure.Rpc;

public class CustomerRpc(ICustomerRepository customerRepository) : ICustomerClient
{
    public async Task<CustomerDetailQuery?> GetByIdAsync(Guid id)
    {
        var customer = await customerRepository.GetByIdAsync(id);
        if (customer == null)
        {
            return null;
        }
        return new CustomerDetailQuery(customer.Id, customer.Name, customer.Phone, customer.Address);
    }
}
