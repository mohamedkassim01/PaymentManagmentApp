using Azure;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using SystemPayment.API.Response;
using SystemPayment.Exceptions;

namespace SystemPayment.API.Exceptions.Middleware
{
	public class ExceptionHandlingMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ExceptionHandlingMiddleware> _logger;
		private readonly IHostEnvironment _env;

		public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IHostEnvironment env)
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
			catch (Exception exception)
			{
				_logger.LogError(exception, "An unhandled exception occurred.");

				(string Message, string Title, int StatusCode) details = exception switch
				{
					InternalServerException => (
						exception.Message,
						exception.GetType().Name,
						StatusCodes.Status500InternalServerError
					),
					ValidationException => (
						exception.Message,
						exception.GetType().Name,
						StatusCodes.Status400BadRequest
					),
					BadRequestException => (
						exception.Message,
						exception.GetType().Name,
						StatusCodes.Status400BadRequest
					),
					NotFoundException => (
						exception.Message,
						exception.GetType().Name,
						StatusCodes.Status404NotFound
					),
					_ => (
						exception.Message,
						exception.GetType().Name,
						StatusCodes.Status500InternalServerError
					),
				};

				List<string> errorList = new List<string>();

				if (exception is ValidationException validationException) errorList.AddRange(validationException.Errors.Select(e => e.ErrorMessage).ToList());
				else errorList.Add(details.Title);

				context.Response.StatusCode = details.StatusCode;
				context.Response.ContentType = "application/json";

				var response = _env.IsDevelopment() ?
					 new ApiResponse<object>(errorList.ToArray().Prepend(details.Message), details.StatusCode)
					: new ApiResponse<object>(errorList.ToArray(), details.StatusCode);


				await context.Response.WriteAsJsonAsync(response);
			}
		}
	}

}
