using Dapper;
using WelderManagement.Domain.Contracts.Repositories;
using WelderManagement.Domain.Entities;
using WelderManagement.Infrastructure.Data;
using WelderManagement.Shared.Responses;
using WelderManagement.Shared.Sorts;
using WelderManagement.Shared.Tools;

namespace WelderManagement.Infrastructure.Repositories;

public class CustomerRepository(IConnectionFactory connFactory) : ICustomerRepository
{
    public async Task InsertAsync(Customer entity)
    {
        var keyword = $"{entity.Name} {entity.Phone}";
        var searchIndex = keyword.ToNgram();
        var sql = """
        INSERT INTO customers (id, name, phone, address, search_index)
        VALUES (@Id, @Name, @Phone, @Address, @SearchIndex)
        """;

        var parameters = new DynamicParameters();
        parameters.Add("Id", entity.Id);
        parameters.Add("Name", entity.Name);
        parameters.Add("Phone", entity.Phone);
        parameters.Add("Address", entity.Address);
        parameters.Add("SearchIndex", searchIndex);

        using var conn = connFactory.CustomerCreateConnection();
        await conn.ExecuteAsync(sql, parameters);
    }

    public async Task<int> CountByIdAsync(Guid id)
    {
        var sql = """
        SELECT COUNT(id)
        FROM customers
        WHERE deleted_at IS NULL AND id = @Id
        """;
        var parameters = new DynamicParameters();
        parameters.Add("Id", id);

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
        var parameters = new DynamicParameters();
        parameters.Add("Phone", phone);

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
        var parameters = new DynamicParameters();
        parameters.Add("DeletedAt", DateTime.UtcNow);
        parameters.Add("Id", id);

        using var conn = connFactory.CustomerCreateConnection();
        await conn.ExecuteAsync(sql, parameters);
    }

    public async Task<Customer?> GetByIdAsync(Guid id)
    {
        var sql = """
        SELECT id, name, phone, address
        FROM customers
        WHERE deleted_at IS NULL AND id = @Id
        """;
        var parameters = new DynamicParameters();
        parameters.Add("Id", id);

        using var conn = connFactory.CustomerCreateConnection();
        var result = await conn.QueryFirstOrDefaultAsync<Customer>(sql, parameters);
        return result;
    }

    public async Task<Pagination<Customer>> GetAllAsync(int page, int pageSize, CustomerSort sort, Direction direction, string? search)
    {
        var whereClause = "WHERE deleted_at IS NULL";
        string? searchKeyword = null;
        if (!string.IsNullOrEmpty(search))
        {
            searchKeyword = search.ToNgram();
            whereClause += " AND MATCH(search_index) AGAINST(@Keyword IN BOOLEAN MODE)";
        }
        var directions = direction == Direction.Ascending ? "ASC" : "DESC";
        var sortBy = sort switch
        {
            CustomerSort.Name => $"ORDER BY name {directions}",
            CustomerSort.Phone => $"ORDER BY phone {directions}",
            CustomerSort.CreatedAt => $"ORDER BY created_at {directions}",
            _ => $"ORDER BY created_at {directions}"
        };

        List<string> queries = [];
        queries.Add($"""
            SELECT id, name
            FROM customers
            {whereClause}
            {sortBy}
            LIMIT @Limit OFFSET @Offset;
            """);
        queries.Add($"SELECT COUNT(id) FROM customers {whereClause};");

        var sql = string.Join(' ', queries);
        var parameters = new DynamicParameters();
        parameters.Add("Limit", pageSize);
        parameters.Add("Offset", (page - 1) * pageSize);
        if (searchKeyword != null)
        {
            parameters.Add("Keyword", searchKeyword);
        }

        using var conn = connFactory.CustomerCreateConnection();
        var multi = await conn.QueryMultipleAsync(sql, parameters);
        var customers = await multi.ReadAsync<Customer>();
        var totalItems = await multi.ReadSingleAsync<int>();
        var contents = customers.ToList().AsReadOnly();
        return new Pagination<Customer>(contents, page, pageSize, totalItems);
    }

    public async Task UpdateAsync(Customer entity)
    {
        var keyword = $"{entity.Name} {entity.Phone}";
        var searchIndex = keyword.ToNgram();
        var sql = """
        UPDATE customers
        SET name = @Name, phone = @Phone, address = @Address, search_index = @SearchIndex
        WHERE id = @Id
        """;
        var parameters = new DynamicParameters();
        parameters.Add("Name", entity.Name);
        parameters.Add("Phone", entity.Phone);
        parameters.Add("Address", entity.Address);
        parameters.Add("SearchIndex", searchIndex);
        parameters.Add("Id", entity.Id);

        using var conn = connFactory.CustomerCreateConnection();
        await conn.ExecuteAsync(sql, parameters);
    }
}
