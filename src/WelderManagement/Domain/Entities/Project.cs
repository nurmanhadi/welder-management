using WelderManagement.Domain.Enums;
using WelderManagement.Shared.Abstractions;

namespace WelderManagement.Domain.Entities;

public class Project : BaseEntity
{
    public string PID { get; set; } = string.Empty;
    public Guid CustomerId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Cost { get; set; }
    public ProjectStatus Status { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
