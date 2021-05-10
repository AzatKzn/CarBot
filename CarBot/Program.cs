using Microsoft.EntityFrameworkCore;
using CarBot.DBContexts;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace CarBot
{
	class Program
	{	
		static int count = 0;
		static void Main(string[] args)
		{
			try
			{
				Configuration.LoadConfig();
				Module.Initialize();
				if (Configuration.IsNeedUpdateDatabase)
				{
					using (var context = new AppDbContext())
					{
						context.Database.Migrate();
					}
				}
				Bot bot = new Bot();
				while (true) ;
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
		//		while (k < 5000000)
		//		{
		//			list.Add(random.Next(97, 112)); // 80%
		//			//list.Add(random.Next(90, 110)); // 50% 
		//			//list.Add(random.Next(88, 103)); // 20%
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
	}
}
