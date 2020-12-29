using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;

namespace PingerForDEX.Configuration
{
	public class Settings
	{		
		public List<SettingNode> GetSettingsList()
		{
			var settingsList = new List<SettingNode>();
			LoadConfiguration().GetSection("HostsList").Bind(settingsList, b => b.BindNonPublicProperties = true);
			
			if (settingsList.Count == 0)
			{
				throw new ArgumentException("SettingsList Error");
			}
			return settingsList;
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
