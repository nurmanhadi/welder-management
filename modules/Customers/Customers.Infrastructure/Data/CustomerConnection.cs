using System.Data;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace Customers.Infrastructure.Data;

public class CustomerConnection(IConfiguration configuration) : ICustomerConnectionFactory
{
    public IDbConnection CustomerCreateConnection()
    {
        return new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));
    }
}
