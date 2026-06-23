using System.Data;
using MySql.Data.MySqlClient;

namespace Customers.Infrastructure.Data;

public class CustomerConnection(string connectionString) : ICustomerConnectionFactory
{
    public IDbConnection CustomerConnect()
    {
        return new MySqlConnection(connectionString);
    }
}
