using PingerForDEX.Configuration;
using PingerForDEX.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;

namespace PingerForDEX.Tools
{
	public class PingerStarter
	{		
		private readonly IServiceProvider _serviceProvider;
		private readonly ILogger _logger;
		private readonly PingerFactory _pingerFactory;
		private readonly SettingsValidator _settingsValidator;
		private readonly Settings _settings;

		public PingerStarter(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
			_logger = _serviceProvider.GetService(typeof(ILogger)) as ILogger ?? throw new ArgumentNullException(nameof(_logger));
			_pingerFactory = _serviceProvider.GetService(typeof(PingerFactory)) as PingerFactory ?? throw new ArgumentNullException(nameof(_pingerFactory));
			_settingsValidator = serviceProvider.GetService(typeof(SettingsValidator)) as SettingsValidator ?? throw new ArgumentNullException(nameof(_settingsValidator));
			_settings = serviceProvider.GetService(typeof(Settings)) as Settings ?? throw new ArgumentNullException(nameof(_settings));
		}

		public async Task StartAsync(CancellationToken token)
		{
			var settingsList = _settings.GetSettingsList();	
			
			try
			{
				foreach (var settingNode in settingsList)
				{
					var validationResult = await _settingsValidator.ValidateAsync(settingNode);

					if (validationResult.IsValid)
					{
						var task = new Task(async () =>
						{
							await Run(settingNode, token);
						}, token);
						task.Start();
					}
					else
					{
						HandleErrors(validationResult);
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.InnerException?.Message);
			}			
		}


		private async Task Run(SettingNode settings, CancellationToken token)
		{
			ResponseData responseData;
			var pinger = _pingerFactory.CreatePinger(settings.ProtocolType);

			while (!token.IsCancellationRequested)
			{
				responseData = await pinger.CheckStatusAsync(settings.HostName);

				if (responseData.StatusWasChanged)
				{
					_logger.LogTheData(responseData.Message);
				}
				Thread.Sleep(settings.Period * 1000);
			}
			Console.WriteLine(settings.HostName + " - " + " pinger is stopped");			
		}

		private void HandleErrors(ValidationResult result)
		{
			foreach (var item in result.Errors)
			{
				_logger.LogTheData(item.ErrorMessage);
				Console.WriteLine("Error! Check setting file.");
			}
		}		
	}
}
