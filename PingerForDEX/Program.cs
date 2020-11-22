using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PingerForDEX.Configuration;
using System;
using System.IO;
using FluentValidation.Results;
using PingerForDEX.Interfaces;
using PingerForDEX.Domain;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Http;

namespace PingerForDEX
{
	class Program
	{

		public static async Task Main(string[] args)
		{
			var services = ConfigureServices();
			var serviceProvider = services.BuildServiceProvider();

			var pingerServicesList = serviceProvider.GetServices<IPinger>();
			var logger = serviceProvider.GetService<ILogger>();			

			foreach (var item in pingerServicesList)
			{
				item.ChangeStatus += logger.LogTheData;
			}		

			var settings = serviceProvider.GetService<ISettings>();
			var settingsValidator = new SettingsValidator();
			var validationResult = await settingsValidator.ValidateAsync(settings as Settings);
			
			if (validationResult.IsValid)
			{
				while (true)
				{
					foreach (var item in pingerServicesList)
					{
						await item.CheckStatusAsynk();
					}
				
					Thread.Sleep(settings.Period * 1000);
				}
			}

			HendleErrors(validationResult, logger);
		}

		private static IServiceCollection ConfigureServices()
		{
			var configuration = LoadConfiguration();
			var servicesCollection = new ServiceCollection();

			servicesCollection.AddSingleton(configuration);
			servicesCollection.AddSingleton<ISettings, Settings>();
			servicesCollection.AddTransient<ILogger, Logger>();
			servicesCollection.AddTransient<Ping>();

			servicesCollection.AddTransient<IPinger, IcmpPinger>();

			servicesCollection.AddTransient<IPinger, TcpPinger>();
			servicesCollection.AddScoped<TcpClient>();

			servicesCollection.AddTransient<IPinger, HttpPinger>();
			servicesCollection.AddScoped<HttpClient>();
			servicesCollection.AddTransient<HttpRequestMessage>();

			return servicesCollection;
		}

		private static IConfiguration LoadConfiguration()
		{
			var configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", false, true);

			return configuration.Build();
		}

		private static void HendleErrors(ValidationResult result, ILogger logger)
		{
			foreach (var item in result.Errors)
			{
				logger.LogTheData(item.ErrorMessage);
				Console.WriteLine("Error! Check setting file.");
			}
		}
	}
}
