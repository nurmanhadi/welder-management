using Customers.Core;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Shared.Responses;

namespace Customers.Endpoints;

public static class Endpoint
{
    public static void MapCustomerEndpoints(this WebApplication web)
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
                throw new ValidationException(validationResult.Errors.First().ToString());
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
        .Produces<ApiResponse<string>>(400);

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
                throw new ValidationException(validationResult.Errors.First().ToString());
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
        .Produces<ApiResponse<string>>(404);

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
    }
}
