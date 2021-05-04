using CarBot.DBContexts;
using CarBot.Models;
using System;
using System.Linq;
using System.Threading;
using TwitchLib.Client.Events;

namespace CarBot
{
	class Races
	{
		public static void SoloRace(OnMessageReceivedArgs e, Bot bot)
		{
			using (var dbContext = new AppDbContext())
			{
				var user = dbContext.Get(e.ChatMessage.UserId);
				if (user != null)
				{
					var history = dbContext.GetLastHistory(user, ActionType.TestDrive);
					TimeSpan timeLeft = new TimeSpan();
					string message;
					if (CanSoloRace(history, ref timeLeft))
					{
						Thread.Sleep(4000);
						var random = new Random();
						var luck = user.Luck;
						double atten = (100 + 1 * random.Next(0, 3)) / (double)100;
						double react = (100 + 1 * random.Next(0, 4)) / (double)100;
						double courage = (100 + 1 * random.Next(-3, 10)) / (double)100;
						double cunning = (100 + 1 * random.Next(1, 4)) / (double)100;
						double min = (atten * react * courage * cunning);
						double expD = random.Next((int)(222), 400) * min * Math.Pow(random.Next(139, 160) / (double)100, luck - 1);
						var exp = (int)expD;
						user.Experience += exp;
						var moneyKF = min;
						var money = (int)(expD * (random.Next(90, 109) / (double)100));
						user.Money += money;
						dbContext.Histories.Add(GetHistory(user, ActionType.TestDrive));
						dbContext.SaveChanges();
						message = string.Format("@{0}, за тест драйв получено {1} опыта и {2} денег", e.ChatMessage.Username, exp, money);
					}
					else					
						message = string.Format("@{0}, следующий тест драйв будет доступен через {1}.", e.ChatMessage.Username, timeLeft.ToString("mm\\:ss"));
					bot.SendMessage(e.ChatMessage.Channel,message);
				}
			}
		}

		private static void DecStrength(User user)
		{
			using (var carContext = new AppDbContext())
			{
				var car = carContext.Cars.Where(x => x.User.Id == user.Id).FirstOrDefault();
				if (car.Strength > 0)
					car.Strength--;
				carContext.SaveChanges();
			}
		}

		private static bool CanSoloRace(History history, ref TimeSpan time)
		{
			if (history == null)
				return true;
			time = DateTime.Now - history.Date;
			if (time.TotalMinutes >= Configuration.TestDriveTimeOutMinutes)
				return true;
			time = TimeSpan.FromMinutes(Configuration.TestDriveTimeOutMinutes) - time;
			return false;
		}

		private static History GetHistory(User user, ActionType actionType)
		{
			return new History() { User = user, ActionType = actionType, Date = DateTime.Now };
		}
	}
}
