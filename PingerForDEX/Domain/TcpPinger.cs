using PingerForDEX.Interfaces;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace PingerForDEX.Domain
{
	public class TcpPinger : IPinger
	{
		
		private string PreviousStatus { get; set; }
		private string NewStatus { get; set; }		
		public string ResponseMesage { get; set ; }
		

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
					NewStatus = "Success";
					ResponseMesage = CreateResponseMessage(NewStatus, hostName);									
				}
				else
				{
					NewStatus = "Fail";
					ResponseMesage = CreateResponseMessage(NewStatus, hostName);					
				}

				respounseData.StatusWasShanged = false;

				if (NewStatus != PreviousStatus)
				{
					respounseData.Message = ResponseMesage;
					respounseData.StatusWasShanged = true;

					PreviousStatus = NewStatus;
				}
			}
			#region catch
			catch (SocketException ex)
			{
				ResponseMesage = CreateResponseMessage(ex.Message, hostName);				
			}
			catch (ObjectDisposedException ex)
			{
				ResponseMesage = CreateResponseMessage(ex.Message, hostName);				
			}
			catch (NullReferenceException ex)
			{
				ResponseMesage = CreateResponseMessage(ex.Message, hostName);				
			}
			catch (ArgumentNullException ex)
			{
				ResponseMesage = CreateResponseMessage(ex.Message, hostName);				
			}
			catch (AggregateException ex)
			{
				ResponseMesage = CreateResponseMessage(ex.Message, hostName);				
			}
			catch (Exception ex)
			{
				ResponseMesage = CreateResponseMessage(ex.Message, hostName);			
			}
			#endregion
			finally
			{
				tcpClient.Close();
			}

			return respounseData;
		}

		public string CreateResponseMessage(string status, string hostName)
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
