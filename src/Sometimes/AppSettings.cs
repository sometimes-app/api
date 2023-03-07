using System;
using static Sometimes.Constants.Constants;

namespace Sometimes
{
	public class AppSettingsConfig
	{
		public static IConfiguration Configuration { get; private set; } = null!;

		public static string ConnectionString => Configuration.GetValue<string>(AppSettings.ConnectionString)!;

		public AppSettingsConfig(IConfiguration configuration)
		{
			Configuration = configuration;
        }
	}
}

