using Customers.Core.Commands;
using Customers.Core.Contracts;
using Customers.Core.Helpers;
using Customers.Endpoints.Dtos;
using Customers.Endpoints.Helpers;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Shared.Exceptions;
using Shared.Responses;
using Shared.Tools;

namespace Customers.Endpoints.Routers;

public static class CustomerRouter
{
    public static void MapCustomerRouters(this IEndpointRouteBuilder web)
    {
        var customers = web.MapGroup("/customers").WithTags("Customers");

        // get customer by id
        customers.MapGet("/{id}", async (
            [FromServices] ICustomerService customerService,
            [FromRoute] Guid id
        ) =>
        {
            var result = await customerService.GetByIdAsync(id);
            return Results.Ok(new ApiResponse<ResponseCustomerDto>(
                Message: "get customer by id success",
                Data: new ResponseCustomerDto(result.Id, result.Name, result.Phone, result.Address)
            ));
        })
        .Produces<ApiResponse<ResponseCustomerDto>>(200)
        .Produces<ApiResponse<string>>(404);

        // add customer
        customers.MapPost("/", async (
            [FromServices] ICustomerService customerService,
            [FromServices] IValidator<AddCustomerDto> validator,
            [FromBody] AddCustomerDto dto
        ) =>
        {
            var validationResult = await validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                throw new ValidatorException(validationResult.Errors.First().ToString());
            }
            var command = new AddCustomerCommand(dto.Name, dto.Phone, dto.Address);
            var result = await customerService.AddAsync(command);
            return Results.Created(
                $"/customers/{result.Id}",
                new ApiResponse<ResponseCustomerDto>(
                    Message: "add customer success",
                    Data: new ResponseCustomerDto(result.Id, result.Name, result.Phone, result.Address)));
        })
        .Produces<ApiResponse<ResponseCustomerDto>>(201)
        .Produces<ApiResponse<string>>(400)
        .Produces<ApiResponse<string>>(409);

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
            var command = new UpdateCustomerCommand(id, dto.Name, dto.Phone, dto.Address);
            var result = await customerService.UpdateAsync(command);
            return Results.Ok(new ApiResponse<ResponseCustomerDto>(
                Message: "update customer success",
                Data: new ResponseCustomerDto(result.Id, result.Name, result.Phone, result.Address)
            ));
        })
        .Produces<ApiResponse<ResponseCustomerDto>>(200)
        .Produces<ApiResponse<string>>(400)
        .Produces<ApiResponse<string>>(404)
        .Produces<ApiResponse<string>>(409);

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
            [FromQuery(Name = "sort")] Sort sort = Sort.CreatedAt,
            [FromQuery(Name = "direction")] Direction direction = Direction.Descending,
            [FromQuery(Name = "s")] string? search = null
        ) =>
        {
            var filter = new CustomerFilter(page, size, (CustomerSort)sort, direction, search);
            var result = await customerService.GetAllAsync(filter);
            var contents = result.Contents.Select(x => new ResponseCustomerDto(x.Id, x.Name, x.Phone, x.Address)).ToList().AsReadOnly();
            return Results.Ok(new ApiResponse<Pagination<ResponseCustomerDto>>(
                Data: new Pagination<ResponseCustomerDto>(contents, result.Page, result.PageSize, result.TotalItems),
                Message: "get all customer success"
            ));
        })
        .Produces<ApiResponse<Pagination<ResponseCustomerDto>>>(200);
    }
}
