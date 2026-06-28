using System.Data;
using MySqlConnector;

namespace WelderManagement.Infrastructure.Data;

public class ConnectionFactory(IConfiguration configuration) : IConnectionFactory
{
    public IDbConnection CustomerCreateConnection()
    {
        return new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));
    }

    public IDbConnection ProjectCreateConnection()
    {
        return new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));
    }
}
