using FluentValidation;
using System.Net;
using System.Text.Json;
using SystemPayment.API.Response;
using SystemPayment.Exceptions;

namespace SystemPayment.API.Middleware
{
	public class ExceptionMiddleware
	{
		private readonly RequestDelegate next;
		private readonly ILogger<ExceptionMiddleware> logger;
		private readonly IHostEnvironment env;

		public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
		{
			this.next = next;
			this.logger = logger;
			this.env = env;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await next(context);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, ex.Message);


				(string Detail, string Title, int StatusCode) details = ex switch
				{
					InternalServerException => (
						ex.Message,
						ex.GetType().Name,
						StatusCodes.Status500InternalServerError
					),
					ValidationException => (
						ex.Message,
						ex.GetType().Name,
						StatusCodes.Status400BadRequest
					),
					BadRequestException => (
						ex.Message,
						ex.GetType().Name,
						StatusCodes.Status400BadRequest
					),
					NotFoundException => (
						ex.Message,
						ex.GetType().Name,
						StatusCodes.Status404NotFound
					),
					_ => (
						ex.Message,
						ex.GetType().Name,
						StatusCodes.Status500InternalServerError
					),
				};

				context.Response.StatusCode = details.StatusCode;
				context.Response.ContentType = "application/json";

				var response = env.IsDevelopment() ?
					 new ApiResponse<object>(new[] { ex.Message, ex.StackTrace.ToString() }, details.StatusCode)
					: new ApiResponse<object>(ex.Message, details.StatusCode);

				var options = new JsonSerializerOptions
				{
					PropertyNamingPolicy = JsonNamingPolicy.CamelCase
				};

				var json = JsonSerializer.Serialize<object>(response, options);

				await context.Response.WriteAsync(json);
			}
		}
	}
}
