using PingerForDEX.Interfaces;
using PingerForDEX.Tools;
using System;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace PingerForDEX.Domain
{
	public class HttpPinger : IPinger
	{
		private readonly HttpRequestMessage _httpRequestMessage;

		private readonly HttpClient _httpClient;

		private HttpStatusCode _previousStatus;
		private HttpStatusCode _newStatus;
		private int _statusCode;
		private string _responseMessage;
		private readonly int _expectedStatus;
		


		public HttpPinger(HttpClient httpClient, HttpRequestMessage httpRequestMessage, int expectedStatus)
		{
			_httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
			_httpRequestMessage = httpRequestMessage ?? throw new ArgumentNullException(nameof(httpRequestMessage));
			_previousStatus = 0;
			_expectedStatus = expectedStatus;
		}
		public async Task<ResponseData> CheckStatusAsync(string hostName, CancellationToken token)
		{
			var uri = new Uri("http://" + hostName);
			_httpRequestMessage.Method = HttpMethod.Head;
			_httpRequestMessage.RequestUri = uri;
			var responseData = new ResponseData();

			try
			{
				var result = await _httpClient.SendAsync(_httpRequestMessage,token);
				
				_newStatus = result.StatusCode;
				_statusCode = (int)_newStatus;
				_responseMessage = CreateResponseMessage(_newStatus.ToString(), hostName);
				responseData.StatusWasChanged = false;

				if ((_newStatus != _previousStatus) && (_statusCode == _expectedStatus))
				{
					responseData.Message = _responseMessage;
					responseData.StatusWasChanged = true;

					_previousStatus = _newStatus;
				}
				else { responseData.Message = "_"; }
			}			
			catch (Exception ex)
			{
				_responseMessage = CreateResponseMessage(ex.InnerException?.Message, hostName);
			}		
			finally
			{
				ResetStatusField();
			}

			return responseData;
		}

		private void ResetStatusField()
		{
			var requestType = _httpRequestMessage.GetType().GetTypeInfo();
			var sendStatusFild = requestType.GetField("_sendStatus", BindingFlags.Instance | BindingFlags.NonPublic);

			if (sendStatusFild != null)
			{
				sendStatusFild.SetValue(_httpRequestMessage, 0);
			}
		}

		private string CreateResponseMessage(string status, string hostName)
		{
			return
				(
				"HTTP " +
				" // " + DateTime.Now +
				" // " + hostName +
				" // " + _statusCode +
				" // " + status
				);
		}

		
	}
}
