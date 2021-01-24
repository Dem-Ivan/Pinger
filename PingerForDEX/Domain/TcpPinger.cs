using PingerForDEX.Interfaces;
using PingerForDEX.Tools;
using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace PingerForDEX.Domain
{
	public class TcpPinger : IPinger
	{

		private string _previousStatus;
		private string _newStatus;
		private string _responseMessage;		

		public async Task<ResponseData> CheckStatusAsync(string hostName, CancellationToken token)
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
					_responseMessage = CreateResponseMessage(_newStatus, hostName);									
				}
				else
				{
					_newStatus = "Fail";
					_responseMessage = CreateResponseMessage(_newStatus, hostName);					
				}

				respounseData.StatusWasChanged = false;

				if (_newStatus != _previousStatus)
				{
					respounseData.Message = _responseMessage;
					respounseData.StatusWasChanged = true;

					_previousStatus = _newStatus;
				}
			}			
			catch (Exception ex)
			{
				_responseMessage = CreateResponseMessage(ex.InnerException?.Message, hostName);			
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
