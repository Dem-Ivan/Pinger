using PingerForDEX.Interfaces;
using System;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace PingerForDEX.Domain
{
	public class HttpPinger : IPinger
	{
		public event Action<string> ChangeStatus;

		private readonly ISettings _settings;

		private readonly HttpRequestMessage _httpRequestMessage;

		private readonly HttpClient _httpClient;

		private HttpStatusCode PreviousStatus { get; set; }
		private HttpStatusCode NewStatus { get; set; }
		private int StatusCode { get; set; }
		public string ResponseMesage { get; set; }

		public HttpPinger(HttpClient httpClient, ISettings settings, HttpRequestMessage httpRequestMessage)
		{
			_settings = settings ?? throw new ArgumentNullException(nameof(settings));
			_httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
			_httpRequestMessage = httpRequestMessage ?? throw new ArgumentNullException(nameof(httpRequestMessage));
			PreviousStatus = 0;//протесть!--------------------------------------_-
		}
		public async Task<string> CheckStatusAsynk()
		{
			var uri = new Uri("http://" + _settings.Host);
			_httpRequestMessage.Method = HttpMethod.Head;
			_httpRequestMessage.RequestUri = uri;

			try
			{
				var result = await _httpClient.SendAsync(_httpRequestMessage);
				NewStatus = result.StatusCode;
				StatusCode = (int)NewStatus;
				ResponseMesage = CreateResponseMessage(NewStatus.ToString());

				if (NewStatus != PreviousStatus)
				{
					ChangeStatus?.Invoke(ResponseMesage);
					PreviousStatus = NewStatus;
				}
			}
			#region catch
			catch (HttpRequestException ex)
			{
				ResponseMesage = CreateResponseMessage(ex.Message);
				ChangeStatus?.Invoke(ResponseMesage);
			}
			catch (InvalidOperationException ex)
			{
				ResponseMesage = CreateResponseMessage(ex.Message);
				ChangeStatus?.Invoke(ResponseMesage);
			}
			catch (UriFormatException ex)
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
				ResetStatusFild();
			}

			return ResponseMesage;
		}

		private void ResetStatusFild()
		{
			var requestType = _httpRequestMessage.GetType().GetTypeInfo();
			var sendStatusFild = requestType.GetField("_sendStatus", BindingFlags.Instance | BindingFlags.NonPublic);

			if (sendStatusFild != null)
			{
				sendStatusFild.SetValue(_httpRequestMessage, 0);
			}
		}

		public string CreateResponseMessage(string status)
		{
			return
				(
				"HTTP " +
				" // " + DateTime.Now +
				" // " + _httpRequestMessage.RequestUri +
				" // " + StatusCode +
				" // " + status
				);
		}
	}
}
