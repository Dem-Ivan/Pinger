using Xunit;
using PingerForDEX.Domain;
using System.Threading.Tasks;

namespace PingerForDEX.Tests
{
	public class TcpPingerTest
	{		
		[Fact]
		public async Task CheckStatusResult()
		{
			//Arrange			
			var tcpPinger = new TcpPinger();

			//Act
			var result = await tcpPinger.CheckStatusAsync("www.ya.ru");

			//Assert
			Assert.NotNull(result.Message);
			Assert.NotEmpty(result.Message);
			Assert.Equal(typeof(string), result.Message.GetType());						
		}	
	}
}
