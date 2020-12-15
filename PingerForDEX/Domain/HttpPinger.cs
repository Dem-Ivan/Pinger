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
		private readonly HttpRequestMessage _httpRequestMessage;

		private readonly HttpClient _httpClient;

		private HttpStatusCode PreviousStatus { get; set; }
		private HttpStatusCode NewStatus { get; set; }
		private int StatusCode { get; set; }
		public string ResponseMesage { get; set; }

		public HttpPinger(HttpClient httpClient, HttpRequestMessage httpRequestMessage)
		{			
			_httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
			_httpRequestMessage = httpRequestMessage ?? throw new ArgumentNullException(nameof(httpRequestMessage));
			PreviousStatus = 0;
		}
		public async Task<ResponseData> CheckStatusAsync(string hostName)
		{
			var uri = new Uri("http://" + hostName);
			_httpRequestMessage.Method = HttpMethod.Head;
			_httpRequestMessage.RequestUri = uri;
			ResponseData respounseData = new ResponseData();

			try
			{
				var result = await _httpClient.SendAsync(_httpRequestMessage);
				NewStatus = result.StatusCode;
				StatusCode = (int)NewStatus;
				ResponseMesage = CreateResponseMessage(NewStatus.ToString(), hostName);
				respounseData.StatusWasShanged = false;

				if (NewStatus != PreviousStatus)
				{
					respounseData.Message = ResponseMesage;
					respounseData.StatusWasShanged = true;					
					
					PreviousStatus = NewStatus;
				}
				
			}
			#region catch
			catch (HttpRequestException ex)
			{
				ResponseMesage = CreateResponseMessage(ex.Message, hostName);				
			}
			catch (InvalidOperationException ex)
			{
				ResponseMesage = CreateResponseMessage(ex.Message, hostName);				
			}
			catch (UriFormatException ex)
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
				ResetStatusFild();
			}

			return respounseData;
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

		public string CreateResponseMessage(string status, string hostName)
		{
			return
				(
				"HTTP " +
				" // " + DateTime.Now +
				" // " + hostName +
				" // " + StatusCode +
				" // " + status
				);
		}
	}
}
