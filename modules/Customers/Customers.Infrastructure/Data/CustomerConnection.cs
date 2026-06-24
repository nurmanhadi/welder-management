using System.Data;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace Customers.Infrastructure.Data;

public class CustomerConnection(IConfiguration configuration) : ICustomerConnectionFactory
{
    public IDbConnection CustomerCreateConnection()
    {
        return new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));
    }
}
