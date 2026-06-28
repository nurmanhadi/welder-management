using WelderManagement.Domain.Entities;
using WelderManagement.Shared.Abstractions;
using WelderManagement.Shared.Responses;
using WelderManagement.Shared.Sorts;
using WelderManagement.Shared.Tools;

namespace WelderManagement.Domain.Contracts.Repositories;

public interface ICustomerRepository : IRepository<Customer>
{
    Task<int> CountByPhoneAsync(string phone);
    Task<Pagination<Customer>> GetAllAsync(int page, int pageSize, CustomerSort sort, Direction direction, string? search);
}
