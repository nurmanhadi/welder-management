namespace Customers.Core;

public record AddCustomerCommand(string Name, string Phone, string Address);
public record UpdateCustomerCommand(Guid Id, string? Name, string? Phone, string? Address);
public record ResponseCustomerCommand(Guid Id, string Name, string Phone, string Address);
