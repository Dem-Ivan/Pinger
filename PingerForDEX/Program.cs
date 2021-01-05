using Microsoft.Extensions.DependencyInjection;
using PingerForDEX.Configuration;
using System;
using PingerForDEX.Domain;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Net.Http;
using PingerForDEX.Interfaces;
using PingerForDEX.Tools;
using System.Threading;

namespace PingerForDEX
{
	internal class Program
	{
		public static async Task Main(string[] args)
		{
			var services = ConfigureServices();
			var serviceProvider = services.BuildServiceProvider();				
			var cts = new CancellationTokenSource();
			var token = cts.Token;
			var pingerStarter = serviceProvider.GetService<PingerStarter>();

			if (pingerStarter != null) await pingerStarter.StartAsync(token);
			else 
			{
				Console.WriteLine("PingerStarter Error");
				return;
			} 
			
			Console.WriteLine("Click <enter> to finish");
			Console.ReadLine();

			cts.Cancel();		
			Console.ReadLine();
		}

		private static IServiceCollection ConfigureServices()
		{
			var serviceCollection = new ServiceCollection();			
			serviceCollection.AddTransient<HttpPinger>();
			serviceCollection.AddSingleton<SettingNode>();		
			serviceCollection.AddTransient<IcmpPinger>();
			serviceCollection.AddTransient<TcpPinger>();
			serviceCollection.AddTransient<HttpRequestMessage>();
			serviceCollection.AddScoped<HttpClient>();
			serviceCollection.AddScoped<TcpClient>();
			serviceCollection.AddTransient<Ping>();
			serviceCollection.AddTransient<ILogger,Logger>();
			serviceCollection.AddTransient<PingerFactory>();
			serviceCollection.AddTransient<SettingsValidator>();
			serviceCollection.AddTransient<PingerStarter>();
			serviceCollection.AddSingleton<Settings>();
			return serviceCollection;
		}		
	}
}
