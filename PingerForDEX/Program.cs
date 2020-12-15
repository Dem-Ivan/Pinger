using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PingerForDEX.Configuration;
using System;
using System.IO;
using FluentValidation.Results;
using PingerForDEX.Domain;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Http;
using System.Collections.Generic;

namespace PingerForDEX
{
	class Program
	{

		public static async Task Main(string[] args)
		{
			var services = ConfigureServices();
			var serviceProvider = services.BuildServiceProvider();			
			var logger = serviceProvider.GetService<Logger>();
			var pingerFactory = serviceProvider.GetService<PingerFactory>();
			var settingsValidator = serviceProvider.GetService<SettingsValidator>();
			object locker = new object();
			Thread thread;
			List<Settings> SettingsList = new List<Settings>();		
			
			LoadConfiguration().GetSection("HostsList").Bind(SettingsList, b => b.BindNonPublicProperties = true);
			

			foreach (var settings in SettingsList)
			{
				var validationResult = await settingsValidator.ValidateAsync(settings as Settings);
				
				if (validationResult.IsValid)
				{
					var pinger = pingerFactory.CreatePinger(settings);
					var pingerStarter = new PingerStarter(pinger, locker, settings);
					thread = new Thread(new ThreadStart(pingerStarter.Start));
					thread.Start();
				}
				HendleErrors(validationResult, logger);
			}			
			Console.ReadLine();		
		}

		private static IServiceCollection ConfigureServices()
		{
			var configuration = LoadConfiguration();

			var serviceCollection = new ServiceCollection();
			serviceCollection.AddSingleton(configuration);
			serviceCollection.AddTransient<HttpPinger>();
			serviceCollection.AddSingleton<Settings>();
			serviceCollection.AddTransient<IcmpPinger>();
			serviceCollection.AddTransient<TcpPinger>();
			serviceCollection.AddTransient<HttpRequestMessage>();
			serviceCollection.AddScoped<HttpClient>();
			serviceCollection.AddScoped<TcpClient>();
			serviceCollection.AddTransient<Ping>();
			serviceCollection.AddTransient<Logger>();
			serviceCollection.AddTransient<PingerFactory>();
			serviceCollection.AddTransient<SettingsValidator>();
			return serviceCollection;
		}
		private static IConfiguration LoadConfiguration()
		{
			var configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", false, true);

			return configuration.Build();
		}

		private static void HendleErrors(ValidationResult result, Logger logger)
		{
			foreach (var item in result.Errors)
			{
				logger.LogTheData(item.ErrorMessage);
				Console.WriteLine("Error! Check setting file.");
			}
		}
	}
}
