﻿using System;
using System.IO;

namespace CarBot
{
	public static class Logger
	{
		private static void Log(string path, string message)
		{
			using (var stream = new StreamWriter(path, true))
			{
				stream.WriteLine(message);
			}
		}
		public static void Log(Exception ex)
		{
			//Console.WriteLine(ex.Message);
			//Console.WriteLine(ex.StackTrace);
			var path = Path.Combine(Directory.GetCurrentDirectory(), "logs", "log.txt");
			var str = "{0} Error:  {1}";
			Log(path, string.Format(str, DateTime.Now, ex.Message) + Environment.NewLine +
					  string.Format(str, DateTime.Now, ex.StackTrace));

		}

		public static void LogInfo(string message)
		{
			var path = Path.Combine(Directory.GetCurrentDirectory(), "logs", "log.txt");
			Log(path, string.Format("{0} Info:  {1}", DateTime.Now, message));
		}

		public static void LogRewardError(Exception ex)
		{
			var path = Path.Combine(Directory.GetCurrentDirectory(), "logs", "rewards.txt");
			var str = "{0} Error:  {1}";
			Log(path, string.Format(str, DateTime.Now, ex.Message) + Environment.NewLine +
					  string.Format(str, DateTime.Now, ex.StackTrace));
		}

		public static void LogRewardInfo(string message)
		{
			var path = Path.Combine(Directory.GetCurrentDirectory(), "logs", "rewards.txt");
			Log(path, string.Format("{0} Info:  {1}", DateTime.Now, message));
		}
	}
}
