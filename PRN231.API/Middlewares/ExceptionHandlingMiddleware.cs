using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using PRN231.API.Enums;
using PRN231.API.Payload.Response;

namespace PRN231.API.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);

            var statusExceptionMap = new Dictionary<int, Exception>
            {
                [(int)HttpStatusCode.Unauthorized] = new UnauthorizedAccessException("Unauthorized access"),
                [(int)HttpStatusCode.Forbidden] = new("Forbidden access")
            };

            if (statusExceptionMap.TryGetValue(context.Response.StatusCode, out var exception))
                await HandleExceptionAsync(context, exception);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        var response = context.Response;

        var errorResponse = new ErrorResponse { Message = exception.Message };
        var exceptionStatusMap = new Dictionary<Type, HttpStatusCode>
        {
            { typeof(UnauthorizedAccessException), HttpStatusCode.Unauthorized },
            { typeof(BadHttpRequestException), HttpStatusCode.BadRequest },
            { typeof(KeyNotFoundException), HttpStatusCode.NotFound },
            { typeof(ValidationException), HttpStatusCode.UnprocessableEntity },
            { typeof(ArgumentNullException), HttpStatusCode.BadRequest },
            { typeof(ArgumentException), HttpStatusCode.BadRequest }
        };

        if (exception.Message == "Forbidden access")
        {
            response.StatusCode = (int)HttpStatusCode.Forbidden;
            errorResponse.StatusCode = (int)HttpStatusCode.Forbidden;
            _logger.LogInformation(exception.Message);
        }
        else if (exceptionStatusMap.TryGetValue(exception.GetType(), out var statusCode))
        {
            response.StatusCode = (int)statusCode;
            errorResponse.StatusCode = (int)statusCode;
            _logger.LogInformation(exception.Message);
        }
        else
        {
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            errorResponse.StatusCode = (int)HttpStatusCode.InternalServerError;
            _logger.LogError(exception.ToString());
        }

        var result = errorResponse.ToString();
        await context.Response.WriteAsync(result);
    }
}

public class AuthorizePolicyAttribute : AuthorizeAttribute
{
    public AuthorizePolicyAttribute(params RoleEnum[] roleEnums)
    {
        var allowedRolesAsString = roleEnums.Select(GetRoleDescription);
        Roles = string.Join(",", allowedRolesAsString);
    }

    private string GetRoleDescription(RoleEnum roleEnum)
    {
        var fi = roleEnum.GetType().GetField(roleEnum.ToString());
        if (fi == null) return roleEnum.ToString();
        var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
        return attributes.Length > 0 ? attributes[0].Description : roleEnum.ToString();
    }
}