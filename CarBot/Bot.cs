using System;
using System.Threading.Tasks;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;
using CarBot.Races;
using TwitchLib.Communication.Events;
using CarBot.BaseTypesExtensions;

namespace CarBot
{
	class Bot
    {
        TwitchClient client;
        readonly string BotName = Config.BotName;
        readonly string OAuth = Config.OAuth;
        readonly string ChannelName = Config.Channel;
        public bool IsOn => isOn;
        bool isOn = true;

        public Bot()
        {
            ConnectionCredentials credentials = new ConnectionCredentials(BotName, OAuth);
            var clientOptions = new ClientOptions
            {
                MessagesAllowedInPeriod = 750,
                ThrottlingPeriod = TimeSpan.FromSeconds(30)
            };
            WebSocketClient customClient = new WebSocketClient(clientOptions);
            client = new TwitchClient(customClient);
            client.Initialize(credentials, ChannelName);
            client.OnConnected += Client_OnConnected;
            client.OnDisconnected += Client_OnDisconnected;
            client.OnMessageReceived += Client_OnMessageReceived;
            client.Connect();
        }

        private void Client_OnDisconnected(object sender, OnDisconnectedEventArgs e)
        {
            try
            {
                Logger.LogInfo("Disconnected.");
                Logger.LogInfo("Try to reconnect...");
                client.Connect();
                Logger.LogInfo("Reconnected...");
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
			}

		}

        private void Client_OnConnected(object sender, OnConnectedArgs e)
        {
            foreach(var channel in client.JoinedChannels)
            {
                Console.WriteLine("Connected to channel: {0}.", channel?.Channel);
			}
		}

        private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
			if (isOn)
				CheckMessage(e);
			else
				AdminCommands(e);
		}

        public async void CheckMessage(OnMessageReceivedArgs e)
        {
            await Task.Run(() =>
            {
                try
                {
                    var message = e.ChatMessage.Message.ToLower().Trim();
                    switch (message)
                    {
                        case "!testdrive": // тестовый зазд
                            SoloRaces.Race(e, this);
                            break;
                        case "!info":  // характеристики пользователя
                            UserActions.ShowInfo(e, this);
                            break;
                        case "!join":  // присоединение к групповому заеду
                            //GroupRaceHandler.JoinToRace(e, this);
                            break;
                        case "!shop": // доступные для покупки машины
                            ShopAction.ShowCarsForSale(e, this);
                            break;                        
                        case "!car": // инфа об авто
                            UserActions.ShowUserCar(e, this);
                            break;
                        case "!start": // команда для начала игры
                            UserActions.CreateUser(e, this);
                            break;
                        case "!game":  // описание игры, с командами
                            ShowGameInfo(e.ChatMessage.Channel);
                            break;
                        default:
                            {
                                if (message.StartsWith("!lvlup")) // улучшение характеристик пользователя
                                {
                                    UserActions.Upgrade(e, this);
                                }
                                else if (message.StartsWith("!iirace")) // гонка с компьютером
                                {
                                    RaceWithAI.Race(e, this);
                                }
                                else if (message.StartsWith("!buy")) // покупка авто
                                {
                                    ShopAction.BuyAuto(e, this);
                                }
                                else if (message.StartsWith("!top")) // показ топа игроков
                                {
                                    Tops.ShowTop(e, this);
                                }

                                AdminCommands(e);
                                break;
                            }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
				}
            });
        }

        void AdminCommands(OnMessageReceivedArgs e)
        {
            if (e.ChatMessage.IsBroadcaster || e.ChatMessage.Username.ToLower().Equals("azatkzn"))
            {
                switch (e.ChatMessage.Message.ToLower())
                {
                    case "!shopchange":
                        if (isOn)
                        ShopAction.ChangeCars(e, this);
                        break;
                    case "!on":
                        isOn = true;
                        SendMessage(e.ChatMessage.Channel, "Бот включен!!!");
                        break;
                    case "!off":
                        isOn = false;
                        SendMessage(e.ChatMessage.Channel, "Бот выключен!!!");
                        break;
                }
            }
        }

        public void ShowGameInfo(string channel)
        {
            var message = "Интерактивная игра, в которой вы можете почувствовать себя настоящим гонщиком. " +
            "Проходите тест драйв, качайте характеристики и покупайте новые машины. Скоро вы " +
            "сможете устраивать заезды друг с другом. Список команд в описании стрима.";
            SendMessage(channel, message);
        }

        public void SendWhisper(string channel, string user, string message)
        {
            var str = "/w {0} {1}".Format(user, message);
            client.SendMessage(channel, str);
		}

        public void SendMessage(string channel, string message)
        {
            client.SendMessage(channel, message);
        }
    }
}

