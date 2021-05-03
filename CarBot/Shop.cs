using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TwitchLib.Client.Events;

namespace CarBot
{
	class Car
	{
		public string IsEnabled { get; set; }
		public string Name { get; set; }
		public string Speed { get; set; }
		public string Mobility { get; set; }
		public string Overclocking { get; set; }
		public string Braking { get; set; }
		public string Cost { get; set; }
	}

	class Shop
	{
		public List<Car> Cars { get; set; }
	}

	static class ShopAction
	{
		public static void ShowCarsForSale(OnMessageReceivedArgs e, Bot bot)
		{
			var cars = GetCars();
			if (cars.Any())
			{
				var message = new StringBuilder();
				message.Append("Доступные тачки: ");
				int i = 1;
				foreach (var car in cars)
				{
					var str = "{0}.{1} - {2}. ";
					message.Append(string.Format(str, i, car.Name, car.Cost));
					i++;
				}
				var response = message.ToString().Trim();
				response = response.Remove(response.Length - 1) + ".";
				bot.SendMessage(e.ChatMessage.Channel, response);
			}
		}

		public static List<Car> GetCars()
		{
			var shop = JsonConvert.DeserializeObject<Shop>(ReadFile());
			return shop.Cars;
		}

		public static string ReadFile()
		{
			var path = Configuration.ShopPath;
			var text = "";
			using (var stream = new StreamReader(path))
			{
				text = stream.ReadToEnd();
			}
			return text;
		}

		public static void BuyAuto(OnMessageReceivedArgs e, Bot bot)
		{
			var message = "";
			bot.SendMessage(e.ChatMessage.Channel, message);
		}
	}
}
