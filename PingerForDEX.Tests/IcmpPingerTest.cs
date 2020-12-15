using Xunit;
using PingerForDEX.Domain;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System;

namespace PingerForDEX.Tests
{	

	public class IcmpPingerTest
	{
		[Fact]
		public void ConstructorTest()
		{
			//Assert
			Assert.Throws<ArgumentNullException>(() => new IcmpPinger(null));			
		}

		[Fact]
		public async Task CheckStatusResult()
		{
			//Arrange					
			var ping = new Ping();
			var icmpPinger = new IcmpPinger(ping);

			//Act
			var result = await icmpPinger.CheckStatusAsync("www.ya.ru");

			//Assert
			Assert.NotNull(result.Message);
			Assert.NotEmpty(result.Message);
			Assert.Equal(typeof(string), result.Message.GetType());
		}

		[Fact]
		public void CreateResponseMessageResult()
		{
			//Arrange			
			var ping = new Ping();
			var icmpPinger = new IcmpPinger(ping);

			//Act
			var result = icmpPinger.CreateResponseMessage("Success", "www.ya.ru");

			//Assert
			Assert.NotNull(result);
			Assert.NotEmpty(result);
			Assert.Equal(typeof(string), result.GetType());			
		}

	}
}
