using Microsoft.Extensions.DependencyInjection;
using PingerForDEX.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace PingerForDEX.Tests
{
	public class SettingsTest
	{
		[Fact]
		public void GetSettingsListTest()
		{
			//Arrange			
			var settings = new Settings();

			//Act
			var result = settings.GetSettingsList();

			//Assert
			Assert.NotNull(result);
			Assert.NotEmpty(result);
		}
	}
}
