using System;
using System.Collections.Generic;
using System.Text;

namespace PingerForDEX.Interfaces
{
	public interface ISettings
	{
		string Host { get; }
		int Period { get; }
		int Port { get; }
	}
}
