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
				Console.ReadLine();

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				Console.WriteLine(ex.StackTrace);
				var path = Path.Combine(Directory.GetCurrentDirectory(), "log.txt");
<<<<<<< HEAD
				using (var stream = new StreamWriter(path, true))
=======
				using (var stream = new StreamWriter(path))
>>>>>>> 2a3cc5e2ddfc9a09f81bb7204f3d2ee032ed813c
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
	}
}
