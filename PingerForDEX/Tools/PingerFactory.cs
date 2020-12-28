using PingerForDEX.Configuration;
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

		public IPinger CreatePinger(string protocolType)
		{
			return protocolType switch
			{
				"ICMP" => _serviceProvider.GetService(typeof(IcmpPinger)) as IcmpPinger,
				"TCP" => _serviceProvider.GetService(typeof(TcpPinger)) as TcpPinger,
				"HTTP" => _serviceProvider.GetService(typeof(HttpPinger)) as HttpPinger,
				_ => throw new ArgumentException("ProtocolType Error"),
			};
		}
	}
}
