using Microsoft.EntityFrameworkCore;
using CarBot.DBContexts;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using CarBot.Models;
using System.Threading;

namespace CarBot
{
	class Program
	{	
		static int count = 0;
		internal static int Minutes { get; set; }
		static void Main(string[] args)
		{
			try
			{
				Config.LoadConfig();
				if (Config.IsNeedUpdateDatabase)
				{
					using (var context = new AppDbContext())
					{
						context.Database.Migrate();
					}
				}
				Module.Initialize();
				var bot = new Bot();
				var pubSubBot = new PubSubBot();
				Minutes = Config.ShopShowMinutes;
				while (true)
				{
					if (Minutes >= Config.ShopShowMinutes && bot.IsOn)
						ShopAction.ChangeCars(Config.Channel, bot);
					Minutes += 5;
					Thread.Sleep(TimeSpan.FromMinutes(5));

				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex);
				if (count < 5)
				{
					count++;
					Main(null);
				}
			}
		}
	}
}
