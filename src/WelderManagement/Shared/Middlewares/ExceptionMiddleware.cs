using WelderManagement.Shared.Exceptions;
using WelderManagement.Shared.Responses;

namespace WelderManagement.Shared.Middlewares;

public static class ExceptionMiddleware
{
    public static IApplicationBuilder UseCustomExceptionMiddleware(this IApplicationBuilder app)
    {
        app.Use(async (context, next) =>
        {
            var logger = context.RequestServices
                    .GetRequiredService<ILoggerFactory>()
                    .CreateLogger("ExceptionMiddleware");
            try
            {
                await next();
            }
            catch (BadHttpRequestException ex)
            {
                logger.LogWarning("{Message}", ex.Message);
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
                logger.LogWarning("{Message}", ex.Message);
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
                logger.LogError(ex, "internal server error");
                context.Response.StatusCode = 500;
                await context.Response.WriteAsJsonAsync(new ApiResponse<string>("internal server error", null));
            }
        });
        return app;
    }
}
