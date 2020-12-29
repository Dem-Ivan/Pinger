using Microsoft.Extensions.DependencyInjection;
using PingerForDEX.Configuration;
using PingerForDEX.Domain;
using PingerForDEX.Interfaces;
using PingerForDEX.Tools;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace PingerForDEX.Tests
{
	public class ServicesEmulator
	{
		public  IServiceCollection ConfigureServices()
		{
			var serviceCollection = new ServiceCollection();
			serviceCollection.AddTransient<HttpPinger>();			
			serviceCollection.AddTransient<IcmpPinger>();
			serviceCollection.AddTransient<TcpPinger>();
			serviceCollection.AddTransient<HttpRequestMessage>();
			serviceCollection.AddScoped<HttpClient>();
			serviceCollection.AddScoped<TcpClient>();
			serviceCollection.AddTransient<Ping>();
			serviceCollection.AddTransient<SettingsValidator>();
			serviceCollection.AddTransient<PingerFactory>();
			serviceCollection.AddTransient<PingerStarter>();
			serviceCollection.AddTransient<ILogger, Logger>();
			serviceCollection.AddSingleton<Settings>();
			return serviceCollection;
		}
	}
}
