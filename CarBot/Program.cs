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
				Configuration.LoadConfig();
				if (Configuration.IsNeedUpdateDatabase)
				{
					using (var context = new AppDbContext())
					{
						context.Database.Migrate();
					}
				}
				Module.Initialize();
				Bot bot = new Bot();
				Minutes = Configuration.ShopShowMinutes;
				while (true)
				{
					if (Minutes >= Configuration.ShopShowMinutes)
						ShopAction.ChangeCars(Configuration.Channel, bot);
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

		//public static void Main(string[] args)
		//{
		//	//Your code goes here
		//	Random random = new Random();
		//	for (int i = 0; i < 10; i++)
		//	{
		//		int k = 0;
		//		var list = new List<int>();
		//		while (k < 50000)
		//		{
		//			//list.Add(random.Next(94, 114)); // 70%	   luck * 1
		//			//list.Add(random.Next(94, 118)); // 73%
		//			//list.Add(random.Next(94, 121)); // 78%   luck * 1
		//			//list.Add(random.Next(94, 129)); // 83%   luck * 2
		//			//list.Add(random.Next(94, 144)); // 88%   luck * 3
		//			//list.Add(random.Next(94, 190)); // 94%   luck * 15
		//			list.Add(Races.RaceWithAI.Test());
		//			k++;
		//		}
		//		//list.ForEach((i) =>
		//		//{
		//		//	Console.WriteLine(i);
		//		//});
		//		var c1 = list.Where(x => x >= 100).Count();
		//		var c2 = list.Count - c1;
		//		Console.WriteLine("{0}	{1}  {2}   {3}%", list.Count, c1, c2, c1 / (double)list.Count * 100);
		//	}
		//}

		//public static void Main(string[] args)
		//{
		//	User user = new User();
		//	Auto auto = new Auto();

		//	for (int i = 1; i < 60; i += 4)
		//	{
		//		int k = 0;
		//		long sum = 0;
		//		user.Attentiveness = i;
		//		user.SpeedReaction = i;
		//		user.Сunning = i;
		//		user.Сourage = i;
		//		user.Luck = 1;
		//		if (i >= 5 && i <= 10)
		//			user.Luck = 2;
		//		if (i > 10 && i <= 20)
		//			user.Luck = 3;
		//		if (i > 20 && i <= 30)
		//			user.Luck = 4;
		//		if (i > 30 && i <= 40)
		//			user.Luck = 5;
		//		if (i > 40 && i <= 50)
		//			user.Luck = 6;
		//		if (i > 50 && i <= 60)
		//			user.Luck = 7;
		//		if (i < 55)
		//		{
		//			auto.Speed = 25;
		//			auto.Mobility = 25;
		//			auto.Overclocking = 25;
		//			auto.Braking = 25;
		//		}
		//		var count = 5000;
		//		while (k < count)
		//		{
		//			k++;
		//			sum += Test(user, auto);
		//		}

		//		Console.WriteLine("	i={0}, luck={1}, auto={2}, mid={3}", i, user.Luck, auto.Speed, sum / count);
		//	}
		//}

		//static int Test(User user, Auto car)
		//{
		//	Random random = new Random();
		//	var s = user.Attentiveness * car.Speed + user.SpeedReaction * car.Mobility +
		//			user.Сunning * car.Overclocking + user.Сourage * car.Braking;
		//	//var luckKF = random.Next(90, 100 + 4 * user.Luck) / (double)100;
		//	return s;
		//}

	}
}
