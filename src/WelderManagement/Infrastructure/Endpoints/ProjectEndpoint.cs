using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using WelderManagement.Applications.Dtos;
using WelderManagement.Domain.Contracts.Services;
using WelderManagement.Shared.Exceptions;
using WelderManagement.Shared.Responses;

namespace WelderManagement.Infrastructure.Endpoints;

public static class ProjectEndpoint
{
    public static IEndpointRouteBuilder MapProjectEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        var projects = routeBuilder.MapGroup("/projects").WithTags("Projects");

        // create draft
        projects.MapPost("/", async (
            [FromServices] IProjectService projectService,
            [FromServices] IValidator<CreateDraftProjectDto> validator,
            [FromBody] CreateDraftProjectDto dto
        ) =>
        {
            var validationResult = await validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                throw new ValidatorException(validationResult.Errors.First().ToString());
            }
            var result = await projectService.CreateDraftAsync(dto);
            return Results.Created($"/projects/{result.Id}",
                new ApiResponse<ProjectDetailDto>(
                    Message: "add draft project success",
                    Data: result
                )
            );
        })
        .Produces<ApiResponse<ProjectDetailDto>>(201)
        .Produces<ApiResponse<string>>(404)
        .Produces<ApiResponse<string>>(422);
        return routeBuilder;
    }
}
