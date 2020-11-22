using PingerForDEX.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PingerForDEX.Domain
{
	public class Logger : ILogger
	{
		public void LogTheData(string message)
		{
			using var write = File.AppendText("log.txt");
			write.WriteLine($"{message}");
			Console.WriteLine(message);
		}		
		
	}
}
