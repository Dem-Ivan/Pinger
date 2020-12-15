using System;
using System.IO;

namespace PingerForDEX.Domain
{
	public class Logger
	{
		public void LogTheData(string message)
		{
			using var write = File.AppendText("log.txt");
			write.WriteLine($"{message}");
			Console.WriteLine(message);
		}		
		
	}
}
