using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace PetStore.RequestLogger
{
	public class RequestLoggingMiddleware
	{
		private readonly RequestDelegate next;
		private readonly ILogger<RequestLoggingMiddleware> logger;

		public RequestLoggingMiddleware(
			RequestDelegate next,
			ILogger<RequestLoggingMiddleware> logger)
		{
			this.next = next;
			this.logger = logger;
		}

		public async Task Invoke(HttpContext context)
		{
			try
			{
				await next(context);
			}
			finally
			{
				logger.LogInformation(
						"Request {method} {url} => {statusCode}",
						context.Request?.Method,
						context.Request?.Path.Value,
						context.Response?.StatusCode);
			}
		}
	}
}