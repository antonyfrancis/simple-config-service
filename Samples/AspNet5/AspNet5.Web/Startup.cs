using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.Framework.Configuration;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Runtime;
using SimpleConfigService.AspNet5;

namespace Sample.AspNet5.Web
{
    public class Startup
    {
	    public IConfiguration Configuration;

	    public Startup(IHostingEnvironment env, IApplicationEnvironment appEnv)
	    {
			var builder = new ConfigurationBuilder(appEnv.ApplicationBasePath);

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
