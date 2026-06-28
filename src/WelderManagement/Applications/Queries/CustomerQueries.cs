namespace WelderManagement.Applications.Queries;

public record CustomerDetailQuery(Guid Id, string Name, string Phone, string Address);
public record CustomerSummaryQuery(Guid Id, string Name);
