using TwitchLib.Client.Events;
using System;
using CarBot.DBContexts;
using System.Linq;
using CarBot.Models;

namespace CarBot
{
	static class ShopAction
	{
		public static void ShowCarsForSale(OnMessageReceivedArgs e, Bot bot)
		{
			//var cars = GetCars();
			//if (cars.Any())
			//{
			//	var message = new StringBuilder();
			//	message.Append("Доступные тачки: ");
			//	int i = 1;
			//	foreach (var car in cars)
			//	{
			//		var str = "{0}.{1} - {2}. ";
			//		message.Append(string.Format(str, i, car.Name, car.Cost));
			//		i++;
			//	}
			//	var response = message.ToString().Trim();
			//	response = response.Remove(response.Length - 1) + ".";
			//	bot.SendMessage(e.ChatMessage.Channel, response);
			//}
		}

		public static void BuyAuto(OnMessageReceivedArgs e, Bot bot)
		{
			string message = string.Empty;
			int id; 
			if (!TryGetId(e.ChatMessage.Message, out id))
				return;
			
			using (var context = new AppDbContext())
			{
				var user = context.GetUser(e.ChatMessage.UserId);
				if (user == null)
					return;
				var auto = context.Autos.FirstOrDefault(a => a.Id == id);
				if (auto == null)
					return;				
				if (!CanBuyAuto(user, auto))
					return;
				var newCar = BuyAuto(user, auto);
				context.Cars.Add(newCar);
				context.SaveChanges();
				message = string.Format("@{0}, поздравляем с покупкой нового авто", e.ChatMessage.Username);
			}

			if (!string.IsNullOrEmpty(message))
				bot.SendMessage(e.ChatMessage.Channel, message);
		}
		
		static UserCar BuyAuto(User user, Auto auto)
		{
			user.Money -= auto.Cost;
			return new UserCar()
			{
				User = user,
				Auto = auto,
				BuyDate = DateTime.Now,
				Strength = 100,
			};
		}

		static bool CanBuyAuto(User user, Auto auto)
		{
			if (user.Money >= auto.Cost)
				return true;
			return false;
		}

		static bool TryGetId(string message, out int id)
		{
			var words = message.Split(' ');
			id = 0;
			if (words.Length < 2)
				return false;				
			try
			{
				id = Convert.ToInt32(words[1]);
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}
