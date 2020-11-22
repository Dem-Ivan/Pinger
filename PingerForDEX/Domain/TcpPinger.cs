using PingerForDEX.Interfaces;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace PingerForDEX.Domain
{
	public class TcpPinger : IPinger
	{
		public event Action<string> ChangeStatus;

		private readonly ISettings _settings;
		private string PreviousStatus { get; set; }
		private string NewStatus { get; set; }		
		public string ResponseMesage { get; set ; }

		public TcpPinger(ISettings settings)
		{
			_settings = settings ?? throw new ArgumentNullException(nameof(settings));
		}

		public async Task<string> CheckStatusAsynk()
		{
			using var tcpClient = new TcpClient();

			try
			{
				var task = Task.Run(() => tcpClient.ConnectAsync(_settings.Host, _settings.Port).Wait(1000));
				var result = await task;

				if (result)
				{
					NewStatus = "Success";
					ResponseMesage = CreateResponseMessage(NewStatus);

					if (NewStatus != PreviousStatus)
					{
						ChangeStatus?.Invoke(ResponseMesage);
						PreviousStatus = NewStatus;
					}
				}
				else
				{
					NewStatus = "Fail";
					ResponseMesage = CreateResponseMessage(NewStatus);

					if (NewStatus != PreviousStatus)
					{
						ChangeStatus?.Invoke(ResponseMesage);
						PreviousStatus = NewStatus;
					}
				}
			}
			#region catch
			catch (SocketException ex)
			{
				ResponseMesage = CreateResponseMessage(ex.Message);
				ChangeStatus?.Invoke(ResponseMesage);			
			}
			catch (ObjectDisposedException ex)
			{
				ResponseMesage = CreateResponseMessage(ex.Message);
				ChangeStatus?.Invoke(ResponseMesage);
			}
			catch (NullReferenceException ex)
			{
				ResponseMesage = CreateResponseMessage(ex.Message);
				ChangeStatus?.Invoke(ResponseMesage);
			}
			catch (ArgumentNullException ex)
			{
				ResponseMesage = CreateResponseMessage(ex.Message);
				ChangeStatus?.Invoke(ResponseMesage);
			}
			catch (AggregateException ex)
			{
				ResponseMesage = CreateResponseMessage(ex.Message);
				ChangeStatus?.Invoke(ResponseMesage);
			}
			catch (Exception ex)
			{
				ResponseMesage = CreateResponseMessage(ex.Message);
				ChangeStatus?.Invoke(ResponseMesage);
			}
			#endregion
			finally
			{
				tcpClient.Close();
			}

			return ResponseMesage;
		}

		public string CreateResponseMessage(string status)
		{
			return
				(
				"TCP " +
				" // " + DateTime.Now +
				" // " + _settings.Host.Normalize() +
				" // " + _settings.Port +
				" // " + status
				);		
		}
	}
}
