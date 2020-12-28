﻿using PingerForDEX.Interfaces;
using PingerForDEX.Tools;
using System;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace PingerForDEX.Domain
{
	public class IcmpPinger : IPinger
	{
		private readonly Ping _ping;
		private IPStatus _previousStatus;
		private IPStatus _newStatus;
		public string _responseMesage;

		public IcmpPinger(Ping ping)
		{
			_ping = ping ?? throw new ArgumentNullException(nameof(ping));
			_previousStatus = IPStatus.Unknown;
		}
		public async Task<ResponseData> CheckStatusAsync(string hostName)
		{
			var uri = hostName;
			ResponseData respounseData = new ResponseData();
			
			try
			{
				var result = await _ping.SendPingAsync(uri, 1000);
				_newStatus = result.Status;
				_responseMesage = CreateResponseMessage(_newStatus.ToString(), hostName);
				respounseData.StatusWasShanged = false;

				if (_newStatus != _previousStatus)
				{
					respounseData.Message = _responseMesage;
					respounseData.StatusWasShanged = true;

					_previousStatus = _newStatus;
				}
			}			
			catch (Exception ex)
			{
				_responseMesage = CreateResponseMessage(ex.InnerException.Message, hostName);				
			}
			return respounseData;
		}

		private string CreateResponseMessage(string status, string hostName)
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
