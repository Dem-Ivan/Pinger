using PingerForDEX.Configuration;
using PingerForDEX.Domain;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace PingerForDEX.Tests
{
	public class HttpPingerTest
	{
		[Fact]
		public async Task CheckStatusResult()
		{
			//Arrange
			var loadConfiguration = new ConfigurationForTest();
			var configuration = loadConfiguration.LoadConfiguration();
			var setting = new Settings(configuration);
			var httpClient = new HttpClient();
			var httpRequestMessage = new HttpRequestMessage();
			var httpPinger = new HttpPinger(httpClient, setting, httpRequestMessage);

			//Act
			var result = await httpPinger.CheckStatusAsynk();

			//Assert
			Assert.NotNull(result);
			Assert.NotEmpty(result);
			Assert.Equal(typeof(string), result.GetType());
			Assert.Contains("// https://ya.ru/ //", result);
		}

		[Fact]
		public void CreateResponseMessageResult()
		{
			//Arrange
			var loadConfiguration = new ConfigurationForTest();
			var configuration = loadConfiguration.LoadConfiguration();
			var setting = new Settings(configuration);
			var httpClient = new HttpClient();
			var httpRequestMessage = new HttpRequestMessage();
			var httpPinger = new HttpPinger(httpClient, setting, httpRequestMessage);

			//Act
			var result = httpPinger.CreateResponseMessage("OK");

			//Assert
			Assert.NotNull(result);
			Assert.NotEmpty(result);
			Assert.Equal(typeof(string), result.GetType());			
		}

	}
}
