﻿using PingerForDEX.Interfaces;
using PingerForDEX.Tools;
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

		private HttpStatusCode _previousStatus;
		private HttpStatusCode _newStatus;
		private int _statusCode;
		public string _responseMesage;

		public HttpPinger(HttpClient httpClient, HttpRequestMessage httpRequestMessage)
		{
			_httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
			_httpRequestMessage = httpRequestMessage ?? throw new ArgumentNullException(nameof(httpRequestMessage));
			_previousStatus = 0;
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
				_newStatus = result.StatusCode;
				_statusCode = (int)_newStatus;
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
