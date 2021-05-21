using CarBot.DBContexts;
using CarBot.DbSetExtensions;
using CarBot.Models;
using System;
using System.Threading;
using TwitchLib.Client.Events;
using CarBot.BaseTypesExtensions;

namespace CarBot.Races
{
	enum Complexity
	{
		Easy = 10,
		Normal = 20,
		Hard = 30,
	}

	static class RaceWithAI
	{
		#region ii race
		public static void Race(OnMessageReceivedArgs e, Bot bot)
		{
			using (var context = new AppDbContext())
			{
				var user = context.Users.Get(e.ChatMessage.UserId);
				if (user == null)
					return;
				var userCar = context.Cars.GetUserCar(user);
				if (userCar == null)
				{
					var noAuto = "@{0}, без автомобиля нельзя выезжать на гонку".Format(e.ChatMessage.Username);
					bot.SendMessage(e.ChatMessage.Channel, noAuto);
					return;
				}

				var history = context.Histories.GetLastUserHistory(user, ActionType.RaceWithAI);
				TimeSpan timeLeft = new TimeSpan();				
				if (!CanRaceWithAI(history, ref timeLeft))
				{
					var left = "@{0}, следующая гонка с ИИ будет доступна через {1}.".Format(e.ChatMessage.Username, timeLeft.ToString("mm\\:ss"));
					bot.SendMessage(e.ChatMessage.Channel, left);
					return;
				}
				Complexity complexity;
				if (!TryGetComplexity(e.ChatMessage.Message, out complexity))
					return;

				var speed = GetSpeed(user, userCar);
				user.Login = e.ChatMessage.Username;
				var message = GetResult(speed, complexity, user);
				context.Histories.CreateAndAddHistory(user, ActionType.RaceWithAI, userCar);

				if (userCar.Strength > 0)
					userCar.Strength -= 0.5f;
				else if (userCar.Strength <= 0)
					userCar.IsActive = false;
				Thread.Sleep(2000);
				context.SaveChanges();
				bot.SendMessage(e.ChatMessage.Channel, message);
			}
		}

		static string GetResult(int speed, Complexity comp, User user)
		{
			bool isWin = false;
			var reward = 0;
			switch (comp)
			{
				case Complexity.Easy:
					isWin = EasyRace(speed, user.Luck, ref reward);
					break;
				case Complexity.Normal:
					isWin = NormalRace(speed, user.Luck, ref reward);
					break;
				case Complexity.Hard:
					isWin = HardRace(speed, user.Luck, ref reward);
					break;
			}
			string message;
			var money = (int)(reward * (new Random().Next(90, 109) / (double)100) / 1.3);
			if (isWin)
			{
				message = "@{0}, ты выиграл в гонке с компьютером и получил {1} опыта и {2} денег".Format(user.Login, reward, money);
				user.VictoriesWithAI++;
			}
			else
				message = "@{0}, ты проиграл в гонке с компьютером, утешительный приз - {1} опыта и {2} денег".Format(user.Login, reward, money);
			user.RaceCountWithAI++;
			user.Experience += reward;
			user.Money += money;
			
			return message;
		}

		static bool HardRace(int speed, int luck, ref int reward)
		{
			Random random = new Random();
			int k = 0;
			if (speed < 300)
				k = random.Next(79, 101 + luck * 1); // 5%
			else if (speed < 600)
				k = random.Next(89, 101 + luck * 1); // 10%
			else if (speed < 900)
				k = random.Next(81, 103 + luck * 1); // 15% 
			else if (speed < 1200)
				k = random.Next(89, 103 + luck * 1); // 25% 
			else if (speed < 1600)
				k = random.Next(84, 107 + luck * 1); // 35% 
			else if (speed < 2000)
				k = random.Next(84, 111 + luck * 1); // 45% 
			else if (speed < 2500)
				k = random.Next(90, 110 + luck * 1); // 55%  
			else if (speed < 3000)
				k = random.Next(91, 113 + luck * 1); // 65% 
			else if (speed < 3500)
				k = random.Next(92, 114 + luck * 1); // 70% 
			else if (speed < 4300)
				k = random.Next(92, 118 + luck * 1); // 75% 
			else if (speed < 4800)
				k = random.Next(92, 134 + luck * 4); // 85%  
			else if (speed < 5200)
				k = random.Next(91, 182 + luck * 15); // 92% 
			else if (speed > 5200)
				k = random.Next(91, 330 + luck * 45); // 97% 

			if (k >= 100)
			{
				reward = random.Next(5000 + luck * 20, 7000 + luck * 10);
				return true;
			}
			if (speed >= 1500)
				reward = random.Next(750 + luck * 15, 900 + luck * 8);
			else if (speed >= 1000)
				reward = random.Next(550 + luck * 15, 700 + luck * 8);
			else if (speed >= 600)
				reward = random.Next(350 + luck * 15, 500 + luck * 8);
			else 
				reward = random.Next(150 + luck * 10, 350 + luck * 5);
			return false;
		}

		static bool NormalRace(int speed, int luck, ref int reward)
		{
			Random random = new Random();
			int k = 0;
			if (speed < 300)
				k = random.Next(93, 103 + luck * 1); // 30%
			else if (speed < 650)
				k = random.Next(93, 104 + luck * 1); // 40%
			else if (speed < 1100)
				k = random.Next(93, 106 + luck * 1); // 50% 
			else if (speed < 1500)
				k = random.Next(93, 109 + luck * 1); // 60%  
			else if (speed < 1900)
				k = random.Next(93, 114 + luck * 1); // 70% 
			else if (speed < 2400)
				k = random.Next(93, 124 + luck * 3); // 80%  
			else if (speed < 2900)
				k = random.Next(93, 134 + luck * 4); // 85% 
			else if (speed < 3500)
				k = random.Next(92, 182 + luck * 15); // 92% 
			else if (speed > 3500)
				k = random.Next(92, 330 + luck * 45); // 97% 

			if (k >= 100)
			{
				reward = random.Next(2500 + luck * 20, 3500 + luck * 10);
				return true;
			}

			reward = random.Next(400 + luck * 13, 600 + luck * 6);
			if (speed <= 600)
				reward = random.Next(150 + luck * 10, 350 + luck * 5);
			return false;
		}

		static bool EasyRace(int speed, int luck, ref int reward)
		{			
			Random random = new Random();
			int k = 0;
			if (speed < 300)
				k = random.Next(94, 114 + luck); // 70%
			else if (speed < 600)
				k = random.Next(94, 118 + luck); // 73%
			else if (speed < 900)
				k = random.Next(94, 121 + luck); // 78% 
			else if (speed < 1200)
				k = random.Next(94, 129 + luck * 2); // 83%
			else if (speed < 1500)
				k = random.Next(94, 144 + luck * 3); // 88%  
			else if (speed > 1500)
				k = random.Next(94, 190 + luck * 15); // 94% 

			if (k >= 100)
			{
				reward = random.Next(1000 + luck * 20, 1400 + luck * 10);
				return true;
			}
			reward = random.Next(150 + luck * 10, 350 + luck * 5);
			return false;
		}

		static int GetSpeed(User user, UserCar userCar)
		{
			var car = userCar.Auto;
			return user.Attentiveness * car.Speed + user.SpeedReaction * car.Mobility +
					user.Сunning * car.Overclocking + user.Сourage * car.Braking;
		}

		static bool TryGetComplexity(string message, out Complexity comp)
		{
			var words = message.Split(' ', StringSplitOptions.RemoveEmptyEntries);
			comp = Complexity.Easy;
			if (words.Length < 2)
				return false;
			switch (words[1])
			{
				case "easy":
					return true;
				case "normal":
					comp = Complexity.Normal;
					return true;
				case "hard":
					comp = Complexity.Hard;
					return true;
			}
			return false;
		}

		static bool CanRaceWithAI(History history, ref TimeSpan time)
		{
			if (history == null)
				return true;
			time = DateTime.Now - history.Date;
			if (time.TotalMinutes >= Config.RaceWithAITimeOutMinutes)
				return true;
			time = TimeSpan.FromMinutes(Config.RaceWithAITimeOutMinutes) - time;
			return false;
		}
		#endregion
	}
}
