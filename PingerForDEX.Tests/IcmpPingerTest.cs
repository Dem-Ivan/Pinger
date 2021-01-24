using Xunit;
using PingerForDEX.Domain;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System;
using System.Threading;

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
			var cts = new CancellationTokenSource();
			var token = cts.Token;

			//Act
			var result = await icmpPinger.CheckStatusAsync("www.ya.ru",token);

			//Assert
			Assert.NotNull(result.Message);
			Assert.NotEmpty(result.Message);
			Assert.Equal(typeof(string), result.Message.GetType());
		}

	}
}
