using WelderManagement.Applications.Queries;

namespace WelderManagement.Domain.Contracts.Clients;

public interface ICustomerClient
{
    Task<CustomerDetailQuery?> GetByIdAsync(Guid id);
}
