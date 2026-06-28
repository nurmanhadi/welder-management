using System.Text.Json.Serialization;
using WelderManagement.Infrastructure.Dependencies;
using WelderManagement.Infrastructure.Endpoints;
using WelderManagement.Shared.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// enum converter
builder.Services.ConfigureHttpJsonOptions(ops => ops.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// dependency
builder.Services
    .AddConnectionDependency()
    .AddMigrationDependency(builder.Configuration)
    .AddValidatorDependency()
    .AddServiceDependency();

builder.Services.AddOpenApi();

var app = builder.Build();

// middleware
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseHttpsRedirection()
.UseCustomExceptionMiddleware();

// endpoints
app
    .MapCustomerEndpoints()
    .MapProjectEndpoints();

app.Run();
