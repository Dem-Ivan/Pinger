using Microsoft.Extensions.Configuration;
using System.IO;


namespace PingerForDEX.Tests
{
	public class ConfigurationForTest
	{
		public  IConfiguration LoadConfiguration()
		{
			var configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", false, true);

			return configuration.Build();
		}
	}
}
