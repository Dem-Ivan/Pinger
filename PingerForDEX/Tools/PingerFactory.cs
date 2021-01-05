using PingerForDEX.Interfaces;
using System;
using PingerForDEX.Domain;

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
					var pinger = _serviceProvider.GetService(typeof(HttpPinger)) as HttpPinger;
					pinger.ExpectedStatus = expectedStatus;
					return pinger;
				default:
					throw new ArgumentException("ProtocolType Error");					
			}			
		}
	}
}
