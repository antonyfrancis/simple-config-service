using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.Framework.Configuration;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Runtime;

namespace Sample.AspNet5.Web
{
    public class Startup
    {
	    public IConfiguration Configuration;

	    public Startup(IHostingEnvironment env, IApplicationEnvironment appEnv)
	    {
			var builder = new ConfigurationBuilder(appEnv.ApplicationBasePath);

			// Load config from config service
			//builder.Add(new ServiceConfigurationSource());

			Configuration = builder.Build();
		}

		public void ConfigureServices(IServiceCollection services)
        {
        }

        public void Configure(IApplicationBuilder app)
        {
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
