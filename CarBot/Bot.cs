using System;
using System.Threading.Tasks;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;
using CarBot.Races;

namespace CarBot
{
	class Bot
    {
        TwitchClient client;
        readonly string BotName = Configuration.BotName;
        readonly string OAuth = Configuration.OAuth;
        readonly string ChannelName = Configuration.Channel;
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
            client.OnMessageReceived += Client_OnMessageReceived;
            client.Connect();
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
                        case "!game":  // описание игры, с командами
                            ShowGameInfo(e.ChatMessage.Channel);
                            break;
                        case "!info":  // характеристики пользователя
                            UserActions.ShowInfo(e, this);
                            break;
                        case "!shop": // доступные для покупки машины
                            ShopAction.ShowCarsForSale(e, this);
                            break;
                        case "!testdrive": // тестовый зазд
                            SoloRaces.Race(e, this);
                            break;
                        case "!car": // инфа об авто
                            UserActions.ShowUserCar(e, this);
                            break;
                        case "!start": // команда для начала игры
                            UserActions.CreateUser(e, this);
                            break;
                        default:
                            {
                                if (message.StartsWith("!lvlup"))
                                {
                                    UserActions.Upgrade(e, this);
                                }
                                else if (message.StartsWith("!iirace")) // гонка с компьютером
                                {
                                    RaceWithAI.Race(e, this);
                                }
                                else if (message.StartsWith("!buy"))
                                {
                                    ShopAction.BuyAuto(e, this);
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
            if (e.ChatMessage.IsBroadcaster)
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
            var str = string.Format("/w {0} {1}", user, message);
            client.SendMessage(channel, str);
		}

        public void SendMessage(string channel, string message)
        {
            client.SendMessage(channel, message);
        }
    }
}

