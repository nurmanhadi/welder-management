using System.Data;

namespace Customers.Infrastructure.Data;

public interface IConnFactory
{
    IDbConnection CustomerConnect();
}
