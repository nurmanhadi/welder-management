using Dapper;
using Projects.Core.Contracts;
using Projects.Core.Entities;
using Projects.Infrastructure.Data;
using Shared.Tools;

namespace Projects.Infrastructure.Repositories;

public class ProjectRepository(IProjectConnectionFactory _db) : IProjectRepository
{
    public async Task AddAsync(Project entity)
    {
        var keyword = $"{entity.PID} {entity.Title}";
        var searchIndex = keyword.ToNgram();
        var sql = """
        INSERT INTO projects(id, pid, customer_id, title, description, cost, status search_index)
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

    public Task<int> CountByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Project?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Project entity)
    {
        throw new NotImplementedException();
    }
}
