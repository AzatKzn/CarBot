using System;
using System.IO;

namespace CarBot
{
	public static class Logger
	{
		public static void Log(Exception ex)
		{
			//Console.WriteLine(ex.Message);
			//Console.WriteLine(ex.StackTrace);
			var path = Path.Combine(Directory.GetCurrentDirectory(), "log.txt");
			using (var stream = new StreamWriter(path, true))
			{
				stream.WriteLine("{0} Error:  {1}", DateTime.Now, ex.Message);
				stream.WriteLine("{0} Error:  {1}", DateTime.Now, ex.StackTrace);
			}
		}

		public static void LogInfo(string message)
		{
			var path = Path.Combine(Directory.GetCurrentDirectory(), "log.txt");
			using (var stream = new StreamWriter(path, true))
			{
				stream.WriteLine("{0} Info:  {1}", DateTime.Now, message);
			}
		}
	}
}
