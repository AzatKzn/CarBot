using Microsoft.EntityFrameworkCore;
using CarBot.DBContexts;
using System;
using System.IO;

namespace CarBot
{
	class Program
	{	
		static int count = 0;
		static void Main(string[] args)
		{
			//var expSum = new long[9];
			//var monSum = new long[9];
			
			//for (int i = 1; i < 8; i ++)
			//{
			//	int k = 0;
			//	while (k < count)
			//	{
			//		Test(i, out long exp, out long money);
			//		expSum[i] += exp;
			//		monSum[i] += money;
			//		k++;
			//	}

			//	Console.WriteLine();
			//	Console.WriteLine();				
			//	Console.WriteLine();
			//	Console.WriteLine();
			//}

			//for (int i = 1; i < 8; i++)
			//{
			//	Console.WriteLine("{0}   {1}", expSum[i] / count, monSum[i] / count);
			//}
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
				Bot bot = new Bot();
				while (true) ;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				Console.WriteLine(ex.StackTrace);
				var path = Path.Combine(Directory.GetCurrentDirectory(), "log.txt");
				using (var stream = new StreamWriter(path, true))
				{
					stream.WriteLine("{0}:  {1}", DateTime.Now, ex.Message);
					stream.WriteLine("{0}:  {1}", DateTime.Now, ex.StackTrace);
				}
				if (count < 5)
				{
					count++;
					Main(null);
				}
			}
		}

		//public static void Test(int luc, out long exp, out long money)
		//{
		//	var random = new Random();
		//	var luck = luc;
		//	var atten = (100 + 1 * random.Next(0, 3)) / (double)100;
		//	var react = (100 + 1 * random.Next(0, 4)) / (double)100;
		//	var courage = (100 + 1 * random.Next(-3, 10)) / (double)100;
		//	var cunning = (100 + 1 * random.Next(1, 4)) / (double)100;
		//	var min = (atten * react * courage * cunning);
		//	var expD = random.Next((int)(222), 400) * min * Math.Pow(random.Next(139, 160) / (double)100, luck - 1);
		//	exp = (long)expD;
		//	var moneyKF = min;
		//	money = (long)(expD * (random.Next(90, 109) / (double) 100));
		//	Console.WriteLine("{0}    {1}", exp, money);
		//}
	}
}
