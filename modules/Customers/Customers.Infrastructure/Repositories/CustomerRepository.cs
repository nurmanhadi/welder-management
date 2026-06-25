using Customers.Core;
using Customers.Infrastructure.Data;
using Dapper;
using Shared.Responses;
using Shared.Tools;

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

    public async Task<Pagination<Customer>> GetAllAsync(CustomerFilter filter)
    {
        var whereClause = "WHERE deleted_at IS NULL";
        string? searchKeyword = null;
        if (!string.IsNullOrEmpty(filter.Search))
        {
            searchKeyword = filter.Search.ToNgram();
            whereClause += " AND MATCH(search_index) AGAINST(@Keyword IN BOOLEAN MODE)";
        }
        var direction = filter.Direction == Direction.Ascending ? "ASC" : "DESC";
        var sortBy = filter.Sort switch
        {
            CustomerSort.Name => $"ORDER BY name {direction}",
            CustomerSort.Phone => $"ORDER BY phone {direction}",
            CustomerSort.CreatedAt => $"ORDER BY created_at {direction}",
            _ => $"ORDER BY created_at {direction}"
        };

        List<string> queries = [];
        queries.Add($"""
            SELECT id, name, phone, address
            FROM customers
            {whereClause}
            {sortBy}
            LIMIT @Limit OFFSET @Offset;
            """);
        queries.Add($"SELECT COUNT(id) FROM customers {whereClause};");

        var sql = string.Join(' ', queries);
        var parameters = new DynamicParameters();
        parameters.Add("Limit", filter.PageSize);
        parameters.Add("Offset", (filter.Page - 1) * filter.PageSize);
        if (searchKeyword != null)
        {
            parameters.Add("Keyword", searchKeyword);
        }

        using var conn = connFactory.CustomerCreateConnection();
        var multi = await conn.QueryMultipleAsync(sql, parameters);
        var customers = await multi.ReadAsync<Customer>();
        var totalItems = await multi.ReadSingleAsync<int>();
        var contents = customers.ToList().AsReadOnly();
        return new Pagination<Customer>(contents, filter.Page, filter.PageSize, totalItems);
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
