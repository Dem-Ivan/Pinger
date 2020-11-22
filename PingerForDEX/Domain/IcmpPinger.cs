using PingerForDEX.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace PingerForDEX.Domain
{
	public class IcmpPinger : IPinger
	{
		public event Action<string> ChangeStatus;

		private readonly ISettings _settings;

		private readonly Ping _ping;
		private IPStatus PreviousStatus { get; set; }
		private IPStatus NewStatus { get; set; }
		public string ResponseMesage { get; set; }

		public IcmpPinger(ISettings settings, Ping ping)
		{
			_settings = settings ?? throw new ArgumentNullException(nameof(settings));
			_ping = ping ?? throw new ArgumentNullException(nameof(ping));
			PreviousStatus = IPStatus.Unknown;
		}
		public async Task<string> CheckStatusAsynk()
		{
			var uri = _settings.Host;

			try
			{
				var result = await _ping.SendPingAsync(uri, 1000);
				NewStatus = result.Status;
				ResponseMesage = CreateResponseMessage(NewStatus.ToString());

				if (NewStatus != PreviousStatus)
				{
					ChangeStatus?.Invoke(ResponseMesage);
					PreviousStatus = NewStatus;
				}

			}
			catch (PingException ex)
			{
				ResponseMesage = CreateResponseMessage(ex.Message);
				ChangeStatus?.Invoke(ResponseMesage);
			}
			catch (Exception ex)
			{
				ResponseMesage = CreateResponseMessage(ex.Message);
				ChangeStatus?.Invoke(ResponseMesage);
			}
			return ResponseMesage;
		}

		public string CreateResponseMessage(string status)
		{
			return 
				(
				"ICMP " +
				" // " + DateTime.Now +
				" // " + _settings.Host +
				" // " + status
				);
		}
	}
}
