using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace PingerForDEX.Configuration
{
	public class SettingsValidator : AbstractValidator<Settings>
	{
		public SettingsValidator()
		{
			RuleFor(x => x.Host).NotEmpty().Must(uri => Uri.TryCreate("http://www." + uri, UriKind.Absolute, out _));

			RuleFor(x => x.Port).InclusiveBetween(1, 65536).NotEmpty();

			RuleFor(x => x.Port).NotEmpty();
		}
	}
}
