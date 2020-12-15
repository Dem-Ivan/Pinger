using PingerForDEX.Configuration;
using PingerForDEX.Interfaces;
using System;
using System.Threading;

namespace PingerForDEX.Domain
{
	public class PingerStarter
	{
		private readonly IPinger _pinger;
		private readonly Settings _settings;
		private readonly Logger logger = new Logger();
		private readonly object _locker;
		
		public PingerStarter(IPinger pinger, object locker, Settings settings)
		{
			_pinger = pinger ?? throw new ArgumentNullException(nameof(pinger));
			_settings = settings ?? throw new ArgumentNullException(nameof(settings));
			_locker = locker ?? throw new ArgumentNullException(nameof(locker));
		}
		public async void Start()
		{
			ResponseData responseData;
			
			while (true)
			{
				responseData = await _pinger.CheckStatusAsync(_settings.HostName);

				if (responseData.StatusWasShanged)
				{
					lock (_locker)
					{
						logger.LogTheData(responseData.Message);
					}
				}							
				Thread.Sleep(_settings.Period * 1000);
			}			
		}
	}
}
