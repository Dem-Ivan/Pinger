using PingerForDEX.Configuration;
using PingerForDEX.Domain;
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
			//Arrange
			var settings = new Settings() { ProtocolType = "TCP" };
			var ping = new Ping();
			var httpClient = new HttpClient();
			var httpRequestMessage = new HttpRequestMessage();

			var icmpPinger = new IcmpPinger(ping);
			var tcpPinger = new TcpPinger();
			var httpPinger = new HttpPinger(httpClient, httpRequestMessage);

			//Assert
			Assert.Throws<ArgumentNullException>(() => new PingerFactory(null, tcpPinger, httpPinger));
			Assert.Throws<ArgumentNullException>(() => new PingerFactory(icmpPinger, null, httpPinger));
			Assert.Throws<ArgumentNullException>(() => new PingerFactory(icmpPinger, tcpPinger, null));
		}

		[Fact]
		public void CreatePingerResult()
		{
			//Arrange
			var settings1 = new Settings() { ProtocolType = "TCP" };
			var settings2 = new Settings() { ProtocolType = "ICMP" };
			var settings3 = new Settings() { ProtocolType = "HTTP" };
			var ping = new Ping();
			var httpClient = new HttpClient();
			var httpRequestMessage = new HttpRequestMessage();			

			var icmpPinger = new IcmpPinger(ping);
			var tcpPinger = new TcpPinger();		
			var httpPinger = new HttpPinger(httpClient, httpRequestMessage);

			var pingerFctory = new PingerFactory(icmpPinger, tcpPinger, httpPinger);

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
