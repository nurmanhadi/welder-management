using System.Data;

namespace Customers.Infrastructure.Data;

public interface ICustomerConnectionFactory
{
    IDbConnection CustomerCreateConnection();
}
