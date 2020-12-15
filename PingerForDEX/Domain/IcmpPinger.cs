using PingerForDEX.Interfaces;
using System;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace PingerForDEX.Domain
{
	public class IcmpPinger : IPinger
	{
		private readonly Ping _ping;
		private IPStatus PreviousStatus { get; set; }
		private IPStatus NewStatus { get; set; }
		public string ResponseMesage { get; set; }

		public IcmpPinger(Ping ping)
		{
			_ping = ping ?? throw new ArgumentNullException(nameof(ping));
			PreviousStatus = IPStatus.Unknown;
		}
		public async Task<ResponseData> CheckStatusAsync(string hostName)
		{
			var uri = hostName;
			ResponseData respounseData = new ResponseData();

			try
			{
				var result = await _ping.SendPingAsync(uri, 1000);
				NewStatus = result.Status;
				ResponseMesage = CreateResponseMessage(NewStatus.ToString(), hostName);
				respounseData.StatusWasShanged = false;

				if (NewStatus != PreviousStatus)
				{
					respounseData.Message = ResponseMesage;
					respounseData.StatusWasShanged = true;

					PreviousStatus = NewStatus;
				}

			}
			catch (PingException ex)
			{
				ResponseMesage = CreateResponseMessage(ex.Message, hostName);				
			}
			catch (Exception ex)
			{
				ResponseMesage = CreateResponseMessage(ex.Message, hostName);				
			}
			return respounseData;
		}

		public string CreateResponseMessage(string status, string hostName)
		{
			return 
				(
				"ICMP " +
				" // " + DateTime.Now +
				" // " + hostName +
				" // " + status
				);
		}
	}
}
