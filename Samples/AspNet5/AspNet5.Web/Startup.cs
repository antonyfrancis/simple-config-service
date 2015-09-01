using System.IO;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.Framework.Configuration;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;
using Microsoft.Framework.Runtime;
using Sample.AspNet5.Web.Models;
using SimpleConfigService.AspNet5;
using IConfiguration = Microsoft.Framework.Configuration.IConfiguration;

namespace Sample.AspNet5.Web
{
	public class Startup
	{
		public IConfiguration Configuration;

		public Startup(IHostingEnvironment env, IApplicationEnvironment appEnv, ILoggerFactory loggerFactory)
		{
			loggerFactory.MinimumLevel = LogLevel.Information;
			loggerFactory.AddConsole();

			var apiConfig = ConfigReader.Get(appEnv.ApplicationBasePath + @"\serviceConfigSource.json");

			var builder = new ConfigurationBuilder(appEnv.ApplicationBasePath)
							.AddJsonFile("config.json")
							.AddEnvironmentVariables();

            builder.Add(new ServiceConfigurationSource(
				apiConfig.ApiKey,
				apiConfig.UriFormat,
				"test",
				"hello"));

			Configuration = builder.Build();
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc();

			services.Configure<AppSettings>(Configuration.GetConfigurationSection("AppSettings"));
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			app.UseMvcWithDefaultRoute();
		}
	}
}
