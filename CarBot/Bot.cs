using System;
using System.Threading.Tasks;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;

namespace CarBot
{
	class Bot
    {
        TwitchClient client;
        readonly string BotName = Configuration.BotName;
        readonly string OAuth = Configuration.OAuth;
        readonly string ChannelName = Configuration.Channel;

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
            CheckMessage(e);           
        }

        public async void CheckMessage(OnMessageReceivedArgs e)
        {
            await Task.Run(() =>
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
                        //ShopAction.ShowCarsForSale(e, this);
                        break;
                    case "!testdrive": // тестовый зазд
                        Races.SoloRace(e, this);
                        break;
                    case "!iirace":  // гонка с компьютером
                        break;
                    case "!car": // инфа об авто
                        break;
                    // можно убрать
                    case "!start": // команда для начала игры
                        UserActions.CreateUser(e, this);
                        break;
                    default:
                        {
                            if (message.StartsWith("!lvlup"))
                            {
                                UserActions.Upgrade(e, this);
                            }
                            if (message.StartsWith("!buy"))
                            {
                                //ShopAction.BuyAuto(e, this);
                            }
                            break;
                        }
                }
            });
        }

        public void ShowGameInfo(string channel)
        {
            var message = "Интерактивная игра, в которой вы можете почувствовать себя настоящим гонщиком. " +
            "Проходите тест драйв, качайте характеристики и покупайте новые машины. Скоро вы " +
            "сможете устраивать заезды друг с другом. Список комманд в описании стрима.";
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
            //Console.WriteLine("Message sent to channel: {0}. Content of message: {1}", channel, message);
        }
    }
}

