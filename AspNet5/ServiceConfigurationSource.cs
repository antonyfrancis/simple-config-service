using System;
using System.IO;
using System.Net.Http;
using System.Text;
using Microsoft.Framework.Configuration;

namespace SimpleConfigService.AspNet5
{
    public class ServiceConfigurationSource : ConfigurationSource
    {
		private readonly string _apiKey;
	    private readonly string _configServiceUriFormat;
	    private readonly string _applicationName;
	    private readonly string _configKey;

		internal Uri ConfigServiceuri => new Uri(string.Format(_configServiceUriFormat, _applicationName, _configKey));

		public ServiceConfigurationSource(string apiKey, string configServiceUriFormat, string applicationName, string configKey)
	    {
			if (string.IsNullOrEmpty(apiKey))
			{
				throw new ArgumentNullException(nameof(apiKey), "ApiKey can't be null or empty");
			}

			if (string.IsNullOrEmpty(configServiceUriFormat))
			{
				throw new ArgumentNullException(nameof(configServiceUriFormat), "ConfigServiceBaseUriFormat can't be null or empty");
			}

			if (string.IsNullOrEmpty(applicationName))
			{
				throw new ArgumentNullException(nameof(applicationName), "ApplicationName can't be null or empty");
			}

			if (string.IsNullOrEmpty(configKey))
			{
				throw new ArgumentNullException(nameof(configKey), "ConfigKey can't be null or empty");
			}

			_apiKey = apiKey;
			_configServiceUriFormat = configServiceUriFormat;
			_applicationName = applicationName;
			_configKey = configKey;
		}

	    public override void Load()
		{
			using (var httpClient = new HttpClient())
			{
				httpClient.DefaultRequestHeaders.Add("x-api-key", _apiKey);

				var data = httpClient.GetStringAsync(ConfigServiceuri).Result;

				if (data != null)
				{
					Data = new CustomJsonConfigurationFileParser().Parse(new MemoryStream(Encoding.UTF8.GetBytes(data)));
				}
			}
		}
    }
}
