namespace Customers.Core.Commands;

public record UpdateCustomerCommand(Guid Id, string? Name, string? Phone, string? Address);
