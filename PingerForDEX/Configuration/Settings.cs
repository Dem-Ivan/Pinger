using PingerForDEX.Interfaces;
using Microsoft.Extensions.Configuration;


namespace PingerForDEX.Configuration
{
	public class Settings : ISettings
	{
		public string Host { get; set; }

		public int Period { get; set; }

		public int Port { get; set; }

		public Settings(IConfiguration configuration)
		{
			configuration.Bind("Settings", this);
		}
	}
}
