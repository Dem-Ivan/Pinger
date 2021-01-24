using PingerForDEX.Domain;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;


namespace PingerForDEX.Tests
{
	
	public class HttpPingerTest
	{
		[Fact]
		public void ConstructorTest()
		{
			//Arrange
			var httpClient = new HttpClient();
			var httpRequestMessage = new HttpRequestMessage();

			//Assert
			Assert.Throws<ArgumentNullException>(() => new HttpPinger(null, httpRequestMessage, 200));
			Assert.Throws<ArgumentNullException>(() => new HttpPinger(httpClient, null, 200));
		}

		[Fact]
		public async Task CheckStatusResult()
		{
			//Arrange			
			var httpClient = new HttpClient();
			var httpRequestMessage = new HttpRequestMessage();
			var httpPinger = new HttpPinger(httpClient, httpRequestMessage, 200);
			var cts = new CancellationTokenSource();
			var token = cts.Token;

			//Act
			var result = await httpPinger.CheckStatusAsync("www.ya.ru",token);

			//Assert
			Assert.NotNull(result.Message);
			Assert.NotEmpty(result.Message);
			Assert.Equal(typeof(string), result.Message.GetType());			
		}		
	}
}
