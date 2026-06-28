using Dapper;
using WelderManagement.Domain.Contracts.Repositories;
using WelderManagement.Domain.Entities;
using WelderManagement.Infrastructure.Data;
using WelderManagement.Shared.Tools;

namespace WelderManagement.Infrastructure.Repositories;

public class ProjectRepository(IConnectionFactory _db) : IProjectRepository
{
    public async Task<int> CountByIdAsync(Guid id)
    {
        var sql = """
        SELECT COUNT(id)
        FROM projects
        WHERE deleted_at IS NULL AND id = $Id
        """;
        var parameters = new DynamicParameters();
        parameters.Add("Id", id);

        using var conn = _db.ProjectCreateConnection();
        int result = await conn.ExecuteScalarAsync<int>(sql, parameters);
        return result;
    }

    public async Task DeleteAsync(Guid id)
    {
        var sql = """
        UPDATE projects SET deleted_at = @DeletedAt
        WHERE id = $Id
        """;
        var parameters = new DynamicParameters();
        parameters.Add("DeletedAt", DateTime.UtcNow);
        parameters.Add("Id", id);

        using var conn = _db.ProjectCreateConnection();
        await conn.ExecuteAsync(sql, parameters);
    }

    public async Task<Project?> GetByIdAsync(Guid id)
    {
        var sql = """
        SELECT id, pid, customer_id, title, description, cost, status, start_date, end_date
        FROM projects
        WHERE deleted_at IS NULL AND id = $Id
        """;
        var parameters = new DynamicParameters();
        parameters.Add("Id", id);

        using var conn = _db.ProjectCreateConnection();
        var result = await conn.QueryFirstOrDefaultAsync<Project>(sql, parameters);
        return result;
    }

    public async Task InsertAsync(Project entity)
    {
        var keyword = $"{entity.PID} {entity.Title}";
        var searchIndex = keyword.ToNgram();
        var sql = """
        INSERT INTO projects(id, pid, customer_id, title, description, cost, status, search_index)
        VALUES(@Id, @Pid, @CustomerId, @Title, @Description, @Cost, @Status, @SearchIndex)
        """;
        var parameters = new DynamicParameters();
        parameters.Add("Id", entity.Id);
        parameters.Add("Pid", entity.PID);
        parameters.Add("CustomerId", entity.CustomerId);
        parameters.Add("Title", entity.Title);
        parameters.Add("Description", entity.Description);
        parameters.Add("Cost", entity.Cost);
        parameters.Add("Status", entity.Status);
        parameters.Add("SearchIndex", searchIndex);

        using var conn = _db.ProjectCreateConnection();
        await conn.ExecuteAsync(sql, parameters);
    }

    public async Task UpdateAsync(Project entity)
    {
        var keyword = $"{entity.PID} {entity.Title}";
        var searchIndex = keyword.ToNgram();
        var sql = """
        UPDATE projects SET title = @Title, description = @Description, cost = @Cost, status = @Status, search_index = @SearchIndex
        WHERE id = @Id
        """;
        var parameters = new DynamicParameters();
        parameters.Add("Title", entity.Title);
        parameters.Add("Description", entity.Description);
        parameters.Add("Cost", entity.Cost);
        parameters.Add("Status", entity.Status);
        parameters.Add("SearchIndex", searchIndex);
        parameters.Add("Id", entity.Id);

        using var conn = _db.ProjectCreateConnection();
        await conn.ExecuteAsync(sql, parameters);
    }
}
