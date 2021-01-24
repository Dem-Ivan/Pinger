using Xunit;
using PingerForDEX.Domain;
using System.Threading.Tasks;
using System.Threading;

namespace PingerForDEX.Tests
{
	public class TcpPingerTest
	{		
		[Fact]
		public async Task CheckStatusResult()
		{
			//Arrange			
			var tcpPinger = new TcpPinger();
			var cts = new CancellationTokenSource();
			var token = cts.Token;

			//Act
			var result = await tcpPinger.CheckStatusAsync("www.ya.ru",token);

			//Assert
			Assert.NotNull(result.Message);
			Assert.NotEmpty(result.Message);
			Assert.Equal(typeof(string), result.Message.GetType());						
		}	
	}
}
