using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarBot.Models;

namespace CarBot
{
	static class Module
	{

		public static void Initialize()
		{
			try
			{
				var cars = GetCars();
				var autos = cars.Select(c => GetAuto(c)).ToList();
				using (var context = new DBContexts.AppDbContext())
				{
					autos.ForEach(auto =>
					{
						var isNotExist = context.Autos.Where(a => a.Id != auto.Id).Count() == 0;
						if (isNotExist)
							context.Autos.Add(auto);
						else
							context.Autos.Update(auto);
					});
					context.SaveChanges();
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex);
			}
		}

		static Auto GetAuto(Car car)
		{
			return new Auto()
			{
				Id = car.Id,
				IsEnabled = Convert.ToBoolean(car.IsEnabled),
				Property = car.Property,
				PropertyValue = car.PropertyValue,
				Name = car.Name,
				Speed = Convert.ToInt32(car.Speed),
				Mobility = Convert.ToInt32(car.Mobility),
				Overclocking = Convert.ToInt32(car.Overclocking),
				Braking = Convert.ToInt32(car.Braking),
				Cost = Convert.ToInt32(car.Cost),
			};
		}

		static List<Car> GetCars()
		{
			var shop = JsonConvert.DeserializeObject<Shop>(ReadFile());
			return shop.Cars;
		}

		static string ReadFile()
		{
			var path = Configuration.ShopPath;
			var text = "";
			using (var stream = new StreamReader(path))
			{
				text = stream.ReadToEnd();
			}
			return text;
		}

		class Car
		{
			public int Id { get; set; }
			public string IsEnabled { get; set; }
			public string Property { get; set; }
			public int PropertyValue { get; set; }
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
	}
	


}
