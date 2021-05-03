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
					if (CanSoloRace(history, ref timeLeft))
					{
						Thread.Sleep(4000);
						var random = new Random();
						var luck = (100 + 1 * random.Next(1, 6)) / (double)100;
						var atten = (100 + 1 * random.Next(0, 3)) / (double)100;
						var react = (100 + 1 * random.Next(0, 4)) / (double)100;
						var courage = (100 + 1 * random.Next(-3, 6)) / (double)100;
						var cunning = (100 + 1 * random.Next(1, 4)) / (double)100;
						var min = (luck * atten * react * courage * cunning);
						var expD = random.Next((int)(min * 200), 500) * min;
						var exp = (int)expD;
						var moneyKF = min;
						var money = (int)(expD * moneyKF);
						user.Experience += exp;
						user.Money += money;
						dbContext.Histories.Add(GetHistory(user, ActionType.TestDrive));
						dbContext.SaveChanges();
						var message = string.Format("@{0}, ты получил за тест драйв {1} опыта и {2} денег", e.ChatMessage.Username, exp, money);
						bot.SendMessage(e.ChatMessage.Channel, message);
					}
					else
					{
						var message = string.Format("@{0}, следующий тест драйв будет доступен через {1}.", e.ChatMessage.Username, timeLeft.ToString("mm\\:ss"));
					}
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
