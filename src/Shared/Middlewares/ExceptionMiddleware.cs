using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Shared.Exceptions;
using Shared.Responses;

namespace Shared.Middlewares;

public static class ExceptionMiddleware
{
    public static void UseExceptionMiddleware(this IApplicationBuilder app)
    {
        app.Use(async (context, next) =>
        {
            try
            {
                await next();
            }
            catch (BadHttpRequestException ex)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsJsonAsync(new ApiResponse<string>(ex.Message, null));
            }
            catch (BadRequestException ex)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsJsonAsync(new ApiResponse<string>(ex.Message, null));
            }
            catch (UnauthorizedAccessException ex)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsJsonAsync(new ApiResponse<string>(ex.Message, null));
            }
            catch (NotFoundException ex)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsJsonAsync(new ApiResponse<string>(ex.Message, null));
            }
            catch (ConflictException ex)
            {
                context.Response.StatusCode = 409;
                await context.Response.WriteAsJsonAsync(new ApiResponse<string>(ex.Message, null));
            }
            catch (ValidatorException ex)
            {
                context.Response.StatusCode = 422;
                await context.Response.WriteAsJsonAsync(new ApiResponse<string>(ex.Message, null));
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsJsonAsync(new ApiResponse<string>(ex.Message, null));
            }
        });
    }
}
