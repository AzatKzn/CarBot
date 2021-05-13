using TwitchLib.Client.Events;
using System;
using CarBot.DBContexts;
using System.Linq;
using CarBot.Models;
using System.Text;
using System.Collections.Generic;

namespace CarBot
{
	static class ShopAction
	{
		public static void ChangeCars(OnMessageReceivedArgs e, Bot bot) => ChangeCars(e.ChatMessage.Channel, bot);
		
		public static void ChangeCars(string channel, Bot bot)
		{
			int min = 0, max = 0;
			using (var context = new AppDbContext())
			{
				min = context.Autos.Min(x => x.Id);
				max = context.Autos.Max(x => x.Id);

				var cars = context.Autos.Where(x => x.IsShow);
				foreach (var car in cars)
					car.IsShow = false;
				context.SaveChanges();				
			}

			var random = new Random().Next(min, max + 1);
			if (random < 0)
				return;
			var list = new List<int>();
			using (var context = new AppDbContext())
			{
				var count = 0;
				while (count < 3)
				{
					random = new Random().Next(min, max + 1);
					if (list.Contains(random))
						continue;
					var car = context.Autos.Where(x => x.IsEnabled && x.Id == random).FirstOrDefault();
					if (car != null)
					{
						car.IsShow = true;
						context.SaveChanges();
						count++;
						list.Add(random);
					}
				}
			}
			Program.Minutes = 0;
			if (!string.IsNullOrEmpty(channel) && bot != null)
				ShowCarsForSale(channel, bot);
		}

		public static void ShowCarsForSale(OnMessageReceivedArgs e, Bot bot) => ShowCarsForSale(e.ChatMessage.Channel, bot);

		public static void ShowCarsForSale(string channel, Bot bot)
		{
			using (var context = new AppDbContext())
			{
				var cars = context.Autos.Where(x => x.IsShow);
				if (!cars.Any())
					return;
				var message = new StringBuilder();
				message.Append("Тачки в продаже: ");
				int i = 1;
				foreach (var car in cars)
				{
					var str = "{0}. {1} (Id={3}) - {2}. ";
					message.Append(string.Format(str, i, car.Name, car.Cost, car.Id));
					i++;
				}
				var response = message.ToString().Trim();
				response = response.Remove(response.Length - 1) + ".";
				bot.SendMessage(channel, response);
			}
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
				var auto = context.Autos.FirstOrDefault(a => a.Id == id && a.IsShow);
				if (auto == null)
					return;
				if (!CanBuyAuto(user, auto, message))
				{
					bot.SendMessage(e.ChatMessage.Channel, message);
					return;
				}

				var newCar = BuyAuto(user, auto);
				var lastCar = context.Cars.Where(c => c.User.Id == user.Id && c.IsActive).FirstOrDefault();
				if (lastCar != null) lastCar.IsActive = false;
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
				IsActive = true,
				Auto = auto,
				BuyDate = DateTime.Now,
				Strength = 100,
			};
		}

		static bool CanBuyAuto(User user, Auto auto, string message)
		{
			var isEnough = false;
			var prop = "";
			switch (auto.Property?.ToLower())
			{
				case "cunning":
					isEnough = user.Сunning >= auto.PropertyValue;
					prop = "хитрость";
					break;
				case "perception":
					isEnough = user.Attentiveness >= auto.PropertyValue;
					prop = "внимательность";
					break;
				case "reaction":
					isEnough = user.SpeedReaction >= auto.PropertyValue;
					prop = "скорость реакции";
					break;
				case "courage":
					isEnough = user.Сourage >= auto.PropertyValue;
					prop = "смелость";
					break;
			}
			if (user.Money >= auto.Cost && isEnough)
				return true;
			
			if (!isEnough)
				message = string.Format("@{0}, для покупки нужна характеристика {1} должна быть равной {2}.", user.Login, prop, auto.PropertyValue);
			else
				message = string.Format("@{0}, недостаточно денег для покупки.", user.Login);

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
