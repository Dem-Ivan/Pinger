using Microsoft.Extensions.DependencyInjection;
using PingerForDEX.Configuration;
using PingerForDEX.Domain;
using PingerForDEX.Tools;
using System;
using System.Net.Http;
using System.Net.NetworkInformation;
using Xunit;

namespace PingerForDEX.Tests
{
	public class PingerFactoryTest
	{
		[Fact]
		public void ConstructorTest()
		{
			//Assert
			Assert.Throws<ArgumentNullException>(() => new PingerFactory(null));			
		}

		[Fact]
		public void CreatePingerResult()
		{
			//Arrange
			var serviceEmulator = new ServicesEmulator();
			var services = serviceEmulator.ConfigureServices();
			var serviceProvider = services.BuildServiceProvider();

			var settings1 = new Settings() { ProtocolType = "TCP" };
			var settings2 = new Settings() { ProtocolType = "ICMP" };
			var settings3 = new Settings() { ProtocolType = "HTTP" };		

			var pingerFctory = new PingerFactory(serviceProvider);

			//Act
			var result1 = pingerFctory.CreatePinger(settings1);
			var result2 = pingerFctory.CreatePinger(settings2);
			var result3 = pingerFctory.CreatePinger(settings3);

			//Assert	
			Assert.Throws<ArgumentNullException>(()=>pingerFctory.CreatePinger(null));
			Assert.Equal(typeof(TcpPinger), result1.GetType());						
			Assert.Equal(typeof(IcmpPinger), result2.GetType());						
			Assert.Equal(typeof(HttpPinger), result3.GetType());
		}
	}
}
