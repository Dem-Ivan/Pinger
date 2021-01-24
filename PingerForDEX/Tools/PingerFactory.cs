using PingerForDEX.Interfaces;
using System;
using PingerForDEX.Domain;
using System.Net.Http;

namespace PingerForDEX.Tools
{
	public class PingerFactory
	{
		private readonly IServiceProvider _serviceProvider;

		public PingerFactory(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
		}

		public IPinger CreatePinger(string protocolType, int expectedStatus)
		{

			switch (protocolType)
			{
				case "ICMP":

					return _serviceProvider.GetService(typeof(IcmpPinger)) as IcmpPinger;

				case "TCP":
					return _serviceProvider.GetService(typeof(TcpPinger)) as TcpPinger;

				case "HTTP":
					return new HttpPinger(_serviceProvider.GetService(typeof(HttpClient)) as HttpClient,
										  _serviceProvider.GetService(typeof(HttpRequestMessage)) as HttpRequestMessage,
										  expectedStatus);
				default:
					throw new ArgumentException("ProtocolType Error");
			}					   		
		}
	}
}
