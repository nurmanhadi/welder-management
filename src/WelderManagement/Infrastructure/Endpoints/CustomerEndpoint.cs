using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using WelderManagement.Applications.Dtos;
using WelderManagement.Domain.Contracts.Services;
using WelderManagement.Shared.Exceptions;
using WelderManagement.Shared.Responses;
using WelderManagement.Shared.Sorts;
using WelderManagement.Shared.Tools;

namespace WelderManagement.Infrastructure.Endpoints;

public static class CustomerEndpoint
{
    public static IEndpointRouteBuilder MapCustomerEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        var customers = routeBuilder.MapGroup("/customers").WithTags("Customers");

        // get customer by id
        customers.MapGet("/{id}", async (
            [FromServices] ICustomerService customerService,
            [FromRoute] Guid id
        ) =>
        {
            var result = await customerService.GetByIdAsync(id);
            return Results.Ok(new ApiResponse<CustomerDetailDto>(
                Message: "get customer by id success",
                Data: result
            ));
        })
        .Produces<ApiResponse<CustomerDetailDto>>(200)
        .Produces<ApiResponse<string>>(404);

        // add customer
        customers.MapPost("/", async (
            [FromServices] ICustomerService customerService,
            [FromServices] IValidator<CreateCustomerDto> validator,
            [FromBody] CreateCustomerDto dto
        ) =>
        {
            var validationResult = await validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                throw new ValidatorException(validationResult.Errors.First().ToString());
            }
            var result = await customerService.CreateAsync(dto);
            return Results.Created(
                $"/customers/{result.Id}",
                new ApiResponse<CustomerDetailDto>(
                    Message: "add customer success",
                    Data: result
                )
            );
        })
        .Produces<ApiResponse<CustomerDetailDto>>(201)
        .Produces<ApiResponse<string>>(409)
        .Produces<ApiResponse<string>>(422);

        // update customer
        customers.MapPut("/{id}", async (
            [FromServices] ICustomerService customerService,
            [FromServices] IValidator<UpdateCustomerDto> validator,
            [FromBody] UpdateCustomerDto dto,
            [FromRoute] Guid id
        ) =>
        {
            var validationResult = await validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                throw new ValidatorException(validationResult.Errors.First().ToString());
            }
            var result = await customerService.UpdateAsync(id, dto);
            return Results.Ok(new ApiResponse<CustomerDetailDto>(
                Message: "update customer success",
                Data: result
            ));
        })
        .Produces<ApiResponse<CustomerDetailDto>>(200)
        .Produces<ApiResponse<string>>(404)
        .Produces<ApiResponse<string>>(409)
        .Produces<ApiResponse<string>>(422);

        // delete customer
        customers.MapDelete("/{id}", async (
            [FromServices] ICustomerService customerService,
            [FromRoute] Guid id
        ) =>
        {
            await customerService.DeleteAsync(id);
            return Results.Ok(new ApiResponse<string>(
                Message: "delete customer success",
                Data: null
            ));
        })
        .Produces<ApiResponse<string>>(200)
        .Produces<ApiResponse<string>>(404);

        // get all
        customers.MapGet("/", async (
            [FromServices] ICustomerService customerService,
            [FromQuery(Name = "page")] int page = 1,
            [FromQuery(Name = "size")] int size = 10,
            [FromQuery(Name = "sort")] CustomerSort sort = CustomerSort.CreatedAt,
            [FromQuery(Name = "direction")] Direction direction = Direction.Descending,
            [FromQuery(Name = "s")] string? search = null
        ) =>
        {
            var result = await customerService.GetAllAsync(page, size, sort, direction, search);
            return Results.Ok(new ApiResponse<Pagination<CustomerSummaryDto>>(
                Message: "get all customer success",
                Data: result
            ));
        })
        .Produces<ApiResponse<Pagination<CustomerSummaryDto>>>(200);

        return routeBuilder;
    }
}
