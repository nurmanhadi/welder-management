namespace WelderManagement.Shared.Abstractions;

public abstract class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string SearchIndex { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DeletedAt { get; set; } = null;
}
