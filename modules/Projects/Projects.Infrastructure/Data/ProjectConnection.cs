using System.Data;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace Projects.Infrastructure.Data;

public class ProjectConnection(IConfiguration configuration) : IProjectConnectionFactory
{
    public IDbConnection ProjectCreateConnection()
    {
        return new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));
    }
}
