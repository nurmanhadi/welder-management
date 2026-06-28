using System.Data;

namespace WelderManagement.Infrastructure.Data;

public interface IConnectionFactory
{
    IDbConnection CustomerCreateConnection();
    IDbConnection ProjectCreateConnection();
}
