using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace Gatherly.App.Middlewares;

public class GlobalExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger _logger;

    public GlobalExceptionHandlingMiddleware(ILogger logger) => _logger = logger;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var problem = new ProblemDetails()
            {
                Status = (int)HttpStatusCode.InternalServerError,
                Type = "Server error",
                Title = "Server error",
                Detail = "An internal server has occured"
            };

            string json = JsonSerializer.Serialize(problem);

            await context.Response.WriteAsync(json);

            context.Response.ContentType = "application/json";
        }
    }
}
