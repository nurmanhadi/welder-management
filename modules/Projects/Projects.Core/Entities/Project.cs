using Projects.Core.Helpers;
using Shared.Abstractions;

namespace Projects.Core.Entities;

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
