using PingerForDEX.Configuration;
using PingerForDEX.Domain;
using System;
using Xunit;

namespace PingerForDEX.Tests
{
	public class PingerStarterTest
	{
		[Fact]
		public void ConstructorTest()
		{
			//Arrange
			var settings = new Settings() { ProtocolType = "TCP" };
			var pinger = new TcpPinger();		

			//Assert
			Assert.Throws<ArgumentNullException>(() => new PingerStarter(null, this, settings));
			Assert.Throws<ArgumentNullException>(() => new PingerStarter(pinger, null, settings));
			Assert.Throws<ArgumentNullException>(() => new PingerStarter(pinger, this, null));
		}
	}
}
