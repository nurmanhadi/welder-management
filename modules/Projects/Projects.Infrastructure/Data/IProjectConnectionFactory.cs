using System.Data;

namespace Projects.Infrastructure.Data;

public interface IProjectConnectionFactory
{
    IDbConnection ProjectCreateConnection();
}
