using FluentValidation;
using System;

namespace PingerForDEX.Configuration
{
	public class SettingsValidator : AbstractValidator<Settings>
	{
		public SettingsValidator()
		{
			RuleFor(x => x.HostName).NotEmpty().Must(uri => Uri.TryCreate("http://www." + uri, UriKind.Absolute, out _));		
		}
	}
}
