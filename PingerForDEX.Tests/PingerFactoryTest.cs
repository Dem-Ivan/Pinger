using Microsoft.Extensions.DependencyInjection;
using PingerForDEX.Domain;
using PingerForDEX.Tools;
using System;
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

			var pingerFactory = serviceProvider.GetService<PingerFactory>();
			int OkStatus = 200;
			//Act
			var tcpResult = pingerFactory.CreatePinger("TCP", OkStatus);
			var icmpResult = pingerFactory.CreatePinger("ICMP", OkStatus);
			var httpResult = pingerFactory.CreatePinger("HTTP", OkStatus);

			//Assert	
			Assert.Throws<ArgumentException> (()=>pingerFactory.CreatePinger(null, OkStatus));
			Assert.Equal(typeof(TcpPinger), tcpResult.GetType());						
			Assert.Equal(typeof(IcmpPinger), icmpResult.GetType());						
			Assert.Equal(typeof(HttpPinger), httpResult.GetType());
		}
	}
}
