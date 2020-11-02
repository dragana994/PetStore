using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Filters;
using System;

namespace PetStore
{
	public class Program
	{
		public static void Main(string[] args)
		{
			Log.Logger = new LoggerConfiguration()
				.WriteTo.Logger(lc => lc
					.Filter.ByExcluding(Matching.FromSource("PetStore.RequestLogger.RequestLoggingMiddleware"))
					.WriteTo.File("logs\\log_.log", rollingInterval: RollingInterval.Day))
				.WriteTo.Logger(lc => lc
					.Filter.ByIncludingOnly(Matching.FromSource("PetStore.RequestLogger.RequestLoggingMiddleware"))
					.WriteTo.File("logs\\request_log_.log", rollingInterval: RollingInterval.Day))
				.CreateLogger();

			try
			{
				Log.Information("Application Starting.");
				CreateHostBuilder(args).Build().Run();
			}
			catch (Exception ex)
			{
				Log.Fatal(ex, "The Application failed to start.");
			}
			finally
			{
				Log.CloseAndFlush();
			}
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
			.UseSerilog()
			.ConfigureWebHostDefaults(webBuilder =>
			{
				webBuilder.UseStartup<Startup>();
			});
	}
}