using Banking.Shared.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Banking.Shared.Middlewares
{
    public sealed class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception);
            }
        }

        private static async Task HandleExceptionAsync(
            HttpContext context,
            Exception exception)
        {
            context.Response.ContentType = "application/json";

            var problemDetails = new ProblemDetails();

            switch (exception)
            {
                case ValidationException validationException:
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;

                    problemDetails.Title = "Validation Error";
                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    problemDetails.Extensions["errors"] =
                        validationException.Errors
                            .Select(x => x.ErrorMessage)
                            .ToArray();

                    break;

                case CustomerServiceUnavailableException customerServiceUnavailableException:
                    context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;

                    problemDetails.Title = "Service Unavailable";
                    problemDetails.Status = StatusCodes.Status503ServiceUnavailable;
                    problemDetails.Detail = customerServiceUnavailableException.Message;

                    break;

                default:
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                    problemDetails.Title = "Server Error";
                    problemDetails.Status = StatusCodes.Status500InternalServerError;
                    problemDetails.Detail = "An unexpected error occurred.";

                    break;
            }

            var response = JsonSerializer.Serialize(problemDetails);

            await context.Response.WriteAsync(response);
        }
    }
}