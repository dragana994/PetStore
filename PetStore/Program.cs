using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace PetStore
{
	public class Program
	{
		public static void Main(string[] args)
		{
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
			.ConfigureWebHostDefaults(webBuilder =>
			{
				webBuilder.UseStartup<Startup>();
			})
			.UseSerilog((hostingContext, loggerConfig) =>
					loggerConfig.ReadFrom.Configuration(hostingContext.Configuration)
			);
	}
}