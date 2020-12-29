using Microsoft.Extensions.DependencyInjection;
using PingerForDEX.Tools;
using System;
using Xunit;
using System.Threading;

namespace PingerForDEX.Tests
{
	public class PingerStarterTest
	{
		[Fact]
		public void ConstructorTest()
		{			
			//Assert
			Assert.Throws<ArgumentNullException>(() => new PingerStarter(null));			
		}
		
		[Fact]
		public void StartAsyncTest()
		{
			//Arrange
			var serviceEmulator = new ServicesEmulator();
			var services = serviceEmulator.ConfigureServices();
			var serviceProvider = services.BuildServiceProvider();
			var pingerStarter = serviceProvider.GetService<PingerStarter>();
			var cts = new CancellationTokenSource();
			var token = cts.Token;

			//Act
			var result = pingerStarter.StartAsync(token);

			//Assert
			Assert.NotNull(result);			
			Assert.True(result.IsCompletedSuccessfully); 
		}
	}
}
