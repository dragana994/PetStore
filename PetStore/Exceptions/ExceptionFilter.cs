using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;

namespace PetStore.Exceptions
{
	public class ExceptionFilter : ExceptionFilterAttribute
	{
		private readonly ILogger<ExceptionFilter> logger;

		public ExceptionFilter(ILogger<ExceptionFilter> logger)
		{
			this.logger = logger;
		}

		public override void OnException(ExceptionContext context)
		{
			var errors = new List<ErrorMessage>();

			if (context.Exception is ValidatorException)
			{
				var exception = context.Exception as ValidatorException;

				context.Exception = null;
				errors = exception.Errors;
				context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
			}
			else if (context.Exception is UnauthorizedAccessException)
			{
				errors.Add(new ErrorMessage(StatusCodes.Status401Unauthorized, "Unauthorized Access!"));

				context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;

				Log.Error(context.Exception.StackTrace);
			}
			else
			{
				errors.Add(new ErrorMessage(StatusCodes.Status500InternalServerError, "An unhandled error occurred."));

				context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

				logger.LogError(context.Exception.StackTrace);
			}

			context.Result = new JsonResult(errors);

			base.OnException(context);
		}
	}
}