namespace Common.Middleware
{
    using Common.Exceptions;
    using Common.Models;
    using FluentValidation;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using System.Net;
    using System.Text.Json;

    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        // Mapeo de excepciones a códigos HTTP
        private readonly Dictionary<Type, HttpStatusCode> _exceptionStatusCodeMap = new()
        {
            { typeof(ValidationException), HttpStatusCode.BadRequest },
            { typeof(CustomException), HttpStatusCode.BadRequest },
            { typeof(InvalidOperationException), HttpStatusCode.BadRequest }
        };

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");

            context.Response.ContentType = "application/json";

            var statusCode = _exceptionStatusCodeMap.ContainsKey(ex.GetType())
                ? (int)_exceptionStatusCodeMap[ex.GetType()]
                : (int)HttpStatusCode.InternalServerError;

            object? errors = null;
            string message = ex.Message;

            switch (ex)
            {
                case ValidationException validationEx:
                    errors = validationEx.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
                    message = "Validation errors";
                    break;

                case CustomException customEx:
                case InvalidOperationException invalidOpEx:
                    // message ya asignado
                    break;

                default:
                    message = _env.IsDevelopment() ? ex.Message : "Internal server error";
                    break;
            }

            var errorDetails = new ErrorDetails
            {
                StatusCode = statusCode,
                Message = message,
                Errors = errors,
                TraceId = context.TraceIdentifier
            };

            context.Response.StatusCode = statusCode;
            var json = JsonSerializer.Serialize(errorDetails, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            await context.Response.WriteAsync(json);
        }
    }
}
