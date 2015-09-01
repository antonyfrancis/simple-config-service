using Microsoft.Framework.ConfigurationModel;

namespace SimpleConfigService.AspNet5
{
    public class ConfigReader
    {
	    public static ApiConfigDetails Get(string configPath)
	    {
			var apiConfig = new ApiConfigDetails();

			var config = new Configuration();
			config.AddJsonFile(configPath);

		    apiConfig.ApiKey = config.Get("SimpleConfigServiceApiKey");
		    apiConfig.UriFormat = config.Get("SimpleConfigServiceUriFormat");

		    return apiConfig;
	    }
    }
}
