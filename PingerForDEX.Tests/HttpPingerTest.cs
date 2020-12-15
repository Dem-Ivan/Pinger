using PingerForDEX.Domain;
using System;
using System.Net.Http;
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
			Assert.Throws<ArgumentNullException>(() => new HttpPinger(null, httpRequestMessage));
			Assert.Throws<ArgumentNullException>(() => new HttpPinger(httpClient, null));
		}

		[Fact]
		public async Task CheckStatusResult()
		{
			//Arrange			
			var httpClient = new HttpClient();
			var httpRequestMessage = new HttpRequestMessage();
			var httpPinger = new HttpPinger(httpClient, httpRequestMessage);

			//Act
			var result = await httpPinger.CheckStatusAsync("www.ya.ru");

			//Assert
			Assert.NotNull(result.Message);
			Assert.NotEmpty(result.Message);
			Assert.Equal(typeof(string), result.Message.GetType());			
		}

		[Fact]
		public void CreateResponseMessageResult()
		{
			//Arrange						
			var httpClient = new HttpClient();
			var httpRequestMessage = new HttpRequestMessage();
			var httpPinger = new HttpPinger(httpClient, httpRequestMessage);

			//Act
			var result = httpPinger.CreateResponseMessage("OK", "www.ya.ru");

			//Assert
			Assert.NotNull(result);
			Assert.NotEmpty(result);
			Assert.Equal(typeof(string), result.GetType());			
		}

	}
}
