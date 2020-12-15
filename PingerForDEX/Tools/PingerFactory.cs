using PingerForDEX.Configuration;
using PingerForDEX.Interfaces;
using System;

namespace PingerForDEX.Domain
{
	public class PingerFactory
	{
		private readonly IcmpPinger _icmpPinger;
		private readonly TcpPinger _tcpPinger;
		private readonly HttpPinger _httpPinger;

		public PingerFactory(IcmpPinger icmpPinger, TcpPinger tcpPinger, HttpPinger httpPinger)
		{
			_icmpPinger = icmpPinger ?? throw new ArgumentNullException(nameof(icmpPinger)); 
			_tcpPinger = tcpPinger ?? throw new ArgumentNullException(nameof(tcpPinger)); 
			_httpPinger = httpPinger ?? throw new ArgumentNullException(nameof(httpPinger)); 
		}

		public IPinger CreatePinger(Settings settings)
		{
			if (settings == null) throw new ArgumentNullException(nameof(settings));
			
			IPinger pinger = null;				
			
			if (settings.ProtocolType == "ICMP")
			{				
				pinger = _icmpPinger;
			}
			else if (settings.ProtocolType == "TCP")
			{				
				pinger = _tcpPinger;
			}
			else if (settings.ProtocolType == "HTTP")
			{				
				pinger = _httpPinger;
			}

			return pinger;
		}
	}
}
