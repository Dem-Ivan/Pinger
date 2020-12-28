using PingerForDEX.Interfaces;
using PingerForDEX.Tools;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace PingerForDEX.Domain
{
	public class TcpPinger : IPinger
	{

		private string _previousStatus;
		private string _newStatus;
		private string _responseMesage;
		

		public async Task<ResponseData> CheckStatusAsync(string hostName)
		{
			using var tcpClient = new TcpClient();
			ResponseData respounseData = new ResponseData();
			try
			{
				var task = Task.Run(() => tcpClient.ConnectAsync(hostName, 80).Wait(1000));
				var result = await task;

				if (result)
				{
					_newStatus = "Success";
					_responseMesage = CreateResponseMessage(_newStatus, hostName);									
				}
				else
				{
					_newStatus = "Fail";
					_responseMesage = CreateResponseMessage(_newStatus, hostName);					
				}

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
			finally
			{
				tcpClient.Close();
			}
			return respounseData;
		}

		private string CreateResponseMessage(string status, string hostName)
		{
			return
				(
				"TCP " +
				" // " + DateTime.Now +
				" // " + hostName +
				" // " + 80 +
				" // " + status
				);		
		}
	}
}
