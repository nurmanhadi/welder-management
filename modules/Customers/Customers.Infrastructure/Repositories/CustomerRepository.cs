using Customers.Core;
using Customers.Infrastructure.Data;
using Dapper;

namespace Customers.Infrastructure.Repositories;

public class CustomerRepository(ICustomerConnectionFactory connFactory) : ICustomerRepository
{
    public async Task AddAsync(Customer entity)
    {
        var sql = """
        INSERT INTO customers (id, name, phone, address)
        VALUES (@Id, @Name, @Phone, @Address)
        """;
        var parameters = new
        {
            Id = entity.Id,
            Name = entity.Name,
            Phone = entity.Phone,
            Address = entity.Address,
        };
        using var conn = connFactory.CustomerCreateConnection();
        await conn.ExecuteAsync(sql, parameters);
    }

    public async Task<int> CountByIdAsync(Guid id)
    {
        var sql = """
        SELECT COUNT(id)
        FROM customers
        WHERE id = @Id
        """;
        var parameters = new
        {
            Id = id
        };
        using var conn = connFactory.CustomerCreateConnection();
        var result = await conn.ExecuteScalarAsync<int>(sql, parameters);
        return result;
    }

    public async Task<int> CountByPhoneAsync(string phone)
    {
        var sql = """
        SELECT COUNT(phone)
        FROM customers
        WHERE phone = @Phone
        """;
        var parameters = new
        {
            Phone = phone
        };
        using var conn = connFactory.CustomerCreateConnection();
        var result = await conn.ExecuteScalarAsync<int>(sql, parameters);
        return result;
    }

    public async Task DeleteAsync(Guid id)
    {
        var sql = """
        UPDATE customers
        SET deleted_at = @DeletedAt
        WHERE id = @Id
        """;
        var parameters = new
        {
            DeletedAt = DateTime.UtcNow,
            Id = id
        };
        using var conn = connFactory.CustomerCreateConnection();
        await conn.ExecuteAsync(sql, parameters);
    }

    public async Task<Customer?> GetByIdAsync(Guid id)
    {
        var sql = """
        SELECT id, name, phone, address
        FROM customers
        WHERE id = @Id
        """;
        var parameters = new
        {
            Id = id
        };
        using var conn = connFactory.CustomerCreateConnection();
        var result = await conn.QueryFirstOrDefaultAsync<Customer>(sql, parameters);
        return result;
    }

    public async Task UpdateAsync(Customer entity)
    {
        var sql = """
        UPDATE customers
        SET name = @Name, phone = @Phone, address = @Address
        WHERE id = @Id
        """;
        var parameters = new
        {
            Id = entity.Id,
            Name = entity.Name,
            Phone = entity.Phone,
            Address = entity.Address,
        };
        using var conn = connFactory.CustomerCreateConnection();
        await conn.ExecuteAsync(sql, parameters);
    }
}
