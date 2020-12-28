using PingerForDEX.Interfaces;
using System;
using System.IO;

namespace PingerForDEX.Tools
{
	public class Logger : ILogger
	{
		private readonly object _locker = new  object();

		public void LogTheData(string message)
		{
			lock (_locker)
			{
				using var write = File.AppendText("log.txt");
				write.WriteLine($"{message}");
				Console.WriteLine(message);
			}
		}

	}
}
