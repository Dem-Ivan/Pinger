using Xunit;
using PingerForDEX.Domain;
using PingerForDEX.Configuration;
using System.Threading.Tasks;
using System.Net.NetworkInformation;

namespace PingerForDEX.Tests
{
	public class IcmpPingerTest
	{
		[Fact]
		public async Task CheckStatusResult()
		{
			//Arrange
			var loadConfiguration = new ConfigurationForTest();
			var configuration = loadConfiguration.LoadConfiguration();
			var setting = new Settings(configuration);
			var ping = new Ping();
			var icmpPinger = new IcmpPinger(setting, ping);

			//Act
			var result = await icmpPinger.CheckStatusAsynk();


			//Assert
			Assert.NotNull(result);
			Assert.NotEmpty(result);
			Assert.Equal(typeof(string), result.GetType());
			Assert.Contains("// www.ya.ru //", result);


		}

		[Fact]
		public void CreateResponseMessageResult()
		{
			//Arrange
			var loadConfiguration = new ConfigurationForTest();
			var configuration = loadConfiguration.LoadConfiguration();
			var setting = new Settings(configuration);
			var ping = new Ping();
			var icmpPinger = new IcmpPinger(setting, ping);

			//Act
			var result = icmpPinger.CreateResponseMessage("Success");

			//Assert
			Assert.NotNull(result);
			Assert.NotEmpty(result);
			Assert.Equal(typeof(string), result.GetType());
			Assert.Contains("// www.ya.ru // Success", result);
		}

	}
}
