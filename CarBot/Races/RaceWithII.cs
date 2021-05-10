using CarBot.DBContexts;
using CarBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client.Events;

namespace CarBot.Races
{
	enum Complexity
	{
		Easy = 10,
		Normal = 20,
		Hard = 30,
	}

	static class RaceWithII
	{
		#region ii race
		public static void Race(OnMessageReceivedArgs e, Bot bot)
		{
			using (var context = new AppDbContext())
			{
				var user = context.GetUser(e.ChatMessage.UserId);
				if (user == null)
					return;
				var userCar = context.GetUserCar(user.Id);
				if (userCar == null)
					return;
				var message = string.Empty;
				Complexity complexity;
				if (!TryGetComplexity(e.ChatMessage.Message, out complexity))
					return;
				var speed = GetSpeed(user, userCar);
				GetResult(speed, complexity);
				// вычислить результат гонки и отправить сообщение
			}
		}

		static void GetResult(double speed, Complexity comp)
		{
			Random random = new Random();
			double kf = 0;

			switch (comp)
			{
				case Complexity.Easy:
					kf = random.Next(97, 112) / (double)100;
					break;
				case Complexity.Normal:
					kf = random.Next(88, 103) / (double)100;
					break;
				case Complexity.Hard:
					kf = random.Next(90, 110) / (double)100;
					break;
			}
		}

		static double GetSpeed(User user, UserCar userCar)
		{
			return 0;
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
		#endregion
	}
}
