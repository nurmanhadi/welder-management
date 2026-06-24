using Customers.Endpoints;
using Customers.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// infrastructure
builder.Services
    .AddCustomersInfrastructure(builder.Configuration);

// migrations
builder.Services
    .AddCustomerMigrations();

// endpoints
builder.Services
    .AddCustomersEndpoints();

builder.Services.AddOpenApi();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapCustomerEndpoints();

app.Run();
