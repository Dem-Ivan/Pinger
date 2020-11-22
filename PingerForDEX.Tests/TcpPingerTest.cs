using Xunit;
using PingerForDEX.Domain;
using PingerForDEX.Configuration;
using System.Threading.Tasks;

namespace PingerForDEX.Tests
{
	public class TcpPingerTest
	{
		[Fact]
		public async Task CheckStatusResult()
		{
			//Arrange
			var loadConfiguration = new ConfigurationForTest();
			var configuration = loadConfiguration.LoadConfiguration();
			var setting = new Settings(configuration);
			var tcpPinger = new TcpPinger(setting);

			//Act
			var result = await tcpPinger.CheckStatusAsynk();


			//Assert
			Assert.NotNull(result);
			Assert.NotEmpty(result);
			Assert.Equal(typeof(string), result.GetType());
			Assert.Contains("// www.ya.ru // 80 //", result);
			
			
		}

		[Fact]
		public void CreateResponseMessageResult()
		{
			//Arrange
			var loadConfiguration = new ConfigurationForTest();
			var configuration = loadConfiguration.LoadConfiguration();
			var setting = new Settings(configuration);
			var tcpPinger = new TcpPinger(setting);

			//Act
			var result = tcpPinger.CreateResponseMessage("Success");

			//Assert
			Assert.NotNull(result);
			Assert.NotEmpty(result);
			Assert.Equal(typeof(string), result.GetType());
			Assert.Contains("// www.ya.ru // 80 // Success", result);
		}
	}
}
