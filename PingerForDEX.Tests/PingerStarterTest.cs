using PingerForDEX.Configuration;
using PingerForDEX.Tools;
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
			var logger = new Logger(this);

			//Assert
			Assert.Throws<ArgumentNullException>(() => new PingerStarter(null,  settings, logger));
			Assert.Throws<ArgumentNullException>(() => new PingerStarter(pinger, null, logger));
			Assert.Throws<ArgumentNullException>(() => new PingerStarter(pinger, settings, null));
		}
	}
}
