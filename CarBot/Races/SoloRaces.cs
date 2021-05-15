using CarBot.DBContexts;
using CarBot.Models;
using System;
using System.Linq;
using System.Threading;
using TwitchLib.Client.Events;

namespace CarBot.Races
{	
	static class SoloRaces
	{		
		#region solo race
		public static void Race(OnMessageReceivedArgs e, Bot bot)
		{
			using (var dbContext = new AppDbContext())
			{
				var user = dbContext.GetUser(e.ChatMessage.UserId);
				if (user != null)
				{
					var history = dbContext.GetLastHistory(user, ActionType.TestDrive);
					TimeSpan timeLeft = new TimeSpan();
					string message;
					if (CanSoloRace(history, ref timeLeft))
					{
						Thread.Sleep(3000);
						int exp, money;
						CountResult(out exp, out money, user);
						user.Experience += exp;
						user.Money += money;
						user.Login = e.ChatMessage.Username;
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

		static void CountResult(out int exp, out int money, User user)
		{
			var random = new Random();
			double atten = (100 + user.Attentiveness * random.Next(0, 3)) / (double)100;
			double react = (100 + user.SpeedReaction * random.Next(0, 4)) / (double)100;
			double courage = (100 + user.Сourage * random.Next(-3, 10)) / (double)100;
			double cunning = (100 + user.Сunning * random.Next(1, 4)) / (double)100;
			if (atten > 1.4)
				atten = 1.4;
			if (react > 1.4)
				react = 1.4;
			if (courage > 1.4)
				courage = 1.4;
			if (courage < 0)
				cunning = 0.65;
			if (cunning > 1.4)
				cunning = 1.4;
			double min = (atten * react * courage * cunning);
			double expD = random.Next(222, 400) * min * Math.Pow(random.Next(139, 160) / (double)100, user.Luck - 1); // rand(222,400) * (rand(139, 160)/100) ^^ (luck - 1) 
			exp = (int)expD;
			money = (int)(expD * (random.Next(90, 109) / (double)100));
		}

		static bool CanSoloRace(History history, ref TimeSpan time)
		{
			if (history == null)
				return true;
			time = DateTime.Now - history.Date;
			if (time.TotalMinutes >= Configuration.TestDriveTimeOutMinutes)
				return true;
			time = TimeSpan.FromMinutes(Configuration.TestDriveTimeOutMinutes) - time;
			return false;
		}

		static History GetHistory(User user, ActionType actionType)
		{
			return new History() { User = user, ActionType = actionType, Date = DateTime.Now };
		}
		#endregion
	}
}
