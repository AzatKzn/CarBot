using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;

namespace CarBot
{
	static class Config
	{
        public static string ConnectionString { get; private set; }
        public static bool IsNeedUpdateDatabase { get; private set; }
        public static string BotName { get; private set; }
        public static string Channel { get; private set; }
        public static string OAuth { get; private set; }
        

        public static string ShopPath { get; private set; }
        public static int TestDriveTimeOutMinutes { get; private set; }
        public static int RaceWithAITimeOutMinutes { get; private set; }
        public static int ShopShowMinutes { get; private set; }
        public static Dictionary<int, int> LVLCost { get; private set; }

        public static void LoadConfig()
		{
            var builder = new ConfigurationBuilder();
            // установка пути к текущему каталогу
            builder.SetBasePath(Directory.GetCurrentDirectory());
            // получаем конфигурацию из файла appsettings.json
            builder.AddJsonFile("appconfig.json");
            // создаем конфигурацию
            var config = builder.Build();
            ConnectionString = config.GetSection("DefaultConnection").Value;
            IsNeedUpdateDatabase = Convert.ToBoolean(config.GetSection("IsNeedUpdateDatabase").Value);
            // настройки для подключения к твичу
            BotName = config.GetSection("TwitchSettings:BotName").Value;
            Channel = config.GetSection("TwitchSettings:Channel").Value;
            OAuth = config.GetSection("TwitchSettings:OAuth").Value;
            ShopPath = Path.Combine(Directory.GetCurrentDirectory(), "AutoShop.json");

            TestDriveTimeOutMinutes = Convert.ToInt32(config.GetSection("TestDriveTimeOutMinutes").Value);
            RaceWithAITimeOutMinutes = Convert.ToInt32(config.GetSection("RaceWithAITimeOutMinutes").Value);
            ShopShowMinutes = Convert.ToInt32(config.GetSection("ShopShowMinutes").Value);
            // стоимость улучшения характеристик
            LVLCost = new Dictionary<int, int>();
            LVLCost.Add(1, Convert.ToInt32(config.GetSection("LVLUPCost:LVL1").Value));
            LVLCost.Add(2, Convert.ToInt32(config.GetSection("LVLUPCost:LVL2").Value));
            LVLCost.Add(3, Convert.ToInt32(config.GetSection("LVLUPCost:LVL3").Value));
            LVLCost.Add(4, Convert.ToInt32(config.GetSection("LVLUPCost:LVL4").Value));
            LVLCost.Add(5, Convert.ToInt32(config.GetSection("LVLUPCost:LVL5").Value));
            LVLCost.Add(6, Convert.ToInt32(config.GetSection("LVLUPCost:LVL6").Value));
            LVLCost.Add(7, Convert.ToInt32(config.GetSection("LVLUPCost:LVL7").Value));
            LVLCost.Add(8, Convert.ToInt32(config.GetSection("LVLUPCost:LVL8").Value));
            LVLCost.Add(9, Convert.ToInt32(config.GetSection("LVLUPCost:LVL9").Value));
            LVLCost.Add(10, Convert.ToInt32(config.GetSection("LVLUPCost:LVLDefault").Value));
        }
    }
}
