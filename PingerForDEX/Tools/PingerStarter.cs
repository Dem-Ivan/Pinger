using PingerForDEX.Configuration;
using PingerForDEX.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace PingerForDEX.Tools
{
	public class PingerStarter
	{
		private readonly ILogger _logger;
		private readonly PingerFactory _pingerFactory;
		private readonly SettingsValidator _settingsValidator;

		public PingerStarter(ILogger logger, PingerFactory pingerFactory, SettingsValidator settingsValidator)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_pingerFactory = pingerFactory ?? throw new ArgumentNullException(nameof(pingerFactory));
			_settingsValidator = settingsValidator ?? throw new ArgumentNullException(nameof(settingsValidator));
		}

		public async Task StartAsync(CancellationToken token)
		{
			List<Settings> settingsList = new List<Settings>();
			LoadConfiguration().GetSection("HostsList").Bind(settingsList, b => b.BindNonPublicProperties = true);// вынести в клас сеттинг
			if (settingsList.Count == 0)
			{
				throw new ArgumentException("Settings Error");
			}
			try
			{
				foreach (var settings in settingsList)
				{
					var validationResult = await _settingsValidator.ValidateAsync(settings);

					if (validationResult.IsValid)
					{
						var task = new Task(async () =>
						{
							await Run(settings, token);
						}, token);
						task.Start();
					}
					else
					{
						HendleErrors(validationResult, _logger);
					}
				}
			}
			catch (OperationCanceledException ex)
			{
				Console.WriteLine(ex.Message);
			}
			
		}


		private async Task Run(Settings settings, CancellationToken token)
		{
			ResponseData responseData;
			var pinger = _pingerFactory.CreatePinger(settings.ProtocolType);

			while (!token.IsCancellationRequested)
			{
				responseData = await pinger.CheckStatusAsync(settings.HostName);

				if (responseData.StatusWasShanged)
				{
					_logger.LogTheData(responseData.Message);
				}
				Thread.Sleep(settings.Period * 1000);
			}
			Console.WriteLine(settings.HostName + " - " + " pinger is stopped");
			throw new OperationCanceledException(token);
		}

		private void HendleErrors(ValidationResult result, ILogger logger)
		{
			foreach (var item in result.Errors)
			{
				logger.LogTheData(item.ErrorMessage);
				Console.WriteLine("Error! Check setting file.");
			}
		}

		private IConfiguration LoadConfiguration()
		{
			var configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", false, true);

			return configuration.Build();
		}
	}
}
