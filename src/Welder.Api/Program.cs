using Customers.Endpoints;
using Customers.Endpoints.Routers;
using Customers.Infrastructure;
using Shared.Middlewares;

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
app.UseExceptionMiddleware();
app.UseHttpsRedirection();

app.MapCustomerRouters();

app.Run();
