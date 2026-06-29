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
        
        // update
        projects.MapPut("/{id}", async (
            [FromServices] IProjectService projectService,
            [FromServices] IValidator<UpdateProjectDto> validator,
            [FromRoute(Name = "id")] Guid id,
            [FromBody] UpdateProjectDto dto
        ) => {
            var validationResult = await validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                throw new ValidatorException(validationResult.Errors.First().ToString());
            }
            var result = await projectService.UpdateAsync(id, dto);
            return Results.Ok(new ApiResponse<ProjectDetailDto>(
                Message: "update project successfully",
                Data: result
            ));
        })
        .Produces<ApiResponse<ProjectDetailDto>>(200)
        .Produces<ApiResponse<string>>(404)
        .Produces<ApiResponse<string>>(409)
        .Produces<ApiResponse<string>>(422);
        
        // get by id
        projects.MapGet("/{id}", async (
            [FromServices] IProjectService projectService,
            [FromRoute(Name = "id")] Guid id
        ) => {
            var result = await projectService.GetByIdAsync(id);
            return Results.Ok(new ApiResponse<ProjectDetailDto>(
                Message: "get project by id successfully",
                Data: result
            ));
        })
        .Produces<ApiResponse<ProjectDetailDto>>(200)
        .Produces<ApiResponse<ProjectDetailDto>>(404);
        
        return routeBuilder;
    }
}
