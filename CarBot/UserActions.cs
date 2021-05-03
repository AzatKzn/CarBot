using CarBot.DBContexts;
using CarBot.Models;
using System;
using System.Linq;
using TwitchLib.Client.Events;

namespace CarBot
{

	class UpgradeResult
    {
        public bool IsSuccess { get; set; }
        public int NewLVL { get; set; }
        public int NeedExp { get; set; }
    }

    /// <summary>
    /// Действия с пользователем
    /// </summary>
    class UserActions
    {
        /// <summary>
        /// Повышение характеристики
        /// </summary>
        /// <remarks>Message начинается с !lvlup и должна содержать имя повышаемой хар-ки</remarks>
        public static void Upgrade(OnMessageReceivedArgs e, Bot bot)
        {            
            using (var context = new AppDbContext())
            {
                var user = context.Get(e.ChatMessage.UserId);
                string propety;
                var message = "";
                UpgradeResult result; 
                if (user != null && IsCorrectCommand(e.ChatMessage.Message.ToLower(), out propety))
                {
                    result = TryUpgrade(user, propety);
                    context.SaveChanges();
                    if (result.IsSuccess)
                    {
                        message = string.Format("@{0}, ваша характеристика \"{1}\" увеличина до {2}.",
                                                e.ChatMessage.Username, propety, result.NewLVL);
                    }
                    else 
                    {
                        message = string.Format("@{0}, для повышения характеристики {1} не хватает {2} опыта.",
                                                e.ChatMessage.Username, propety, result.NeedExp);
                    }
                    bot.SendMessage(e.ChatMessage.Channel, message);
                }
                
            }
        }

        static UpgradeResult TryUpgrade(User user, string upgradePropety)
        {
            int remove = 0;
            bool isCanUpdate = false;
            int newLVL = 0;
            switch (upgradePropety)
            {
                case "perception":
                    {
                        isCanUpdate = IsEnoughMoney(user.Experience, user.Attentiveness, out remove);
                        if (isCanUpdate)
                        {
                            user.Experience -= remove;
                            user.Attentiveness++;
                            newLVL = user.Attentiveness;
                        }
                        break;
                    }
                case "reaction":
                    {
                        isCanUpdate = IsEnoughMoney(user.Experience, user.SpeedReaction, out remove);
                        if (isCanUpdate)
                        {
                            user.Experience -= remove;
                            user.SpeedReaction++;
                            newLVL = user.SpeedReaction;
                        }
                        break;
                    }
                case "cunning":
                    {
                        isCanUpdate = IsEnoughMoney(user.Experience, user.Сunning, out remove);
                        if (isCanUpdate)
                        {
                            user.Experience -= remove;
                            user.Сunning++;
                            newLVL = user.Сunning;
                        }
                        break;
                    }
                case "boldness":
                    {
                        isCanUpdate = IsEnoughMoney(user.Experience, user.Сourage, out remove);
                        if (isCanUpdate)
                        {
                            user.Experience -= remove;
                            user.Сourage++;
                            newLVL = user.Сourage;
                        }
                        break;
                    }
                case "luck":
                    {
                        isCanUpdate = IsEnoughMoney(user.Experience, user.Luck, out remove);
                        if (isCanUpdate)
                        {
                            user.Experience -= remove;
                            user.Luck++;
                            newLVL = user.Luck;
                        }
                        break;
                    }
            }
            int needExp = 0;
            if (!isCanUpdate)
                needExp = remove - (int)user.Experience;
            return new UpgradeResult() { IsSuccess = isCanUpdate, NewLVL = newLVL, NeedExp = needExp };
		}

        /// <summary>
        /// Проверка на наличие опыта
        /// </summary>
        /// <param name="exp">Общее кол-во опыта</param>
        /// <param name="currentLVL">текущий уровень хар-ки</param>
        /// <param name="remove">Количество опыта на апгрейд</param>
        /// <returns></returns>
        static bool IsEnoughMoney(long exp, int currentLVL, out int remove)
        {
            remove = 0;
            if (currentLVL < 9)
                remove = Configuration.LVLCost[currentLVL];
            else if (currentLVL > 9)
                remove = Configuration.LVLCost[10];
            return exp >= remove && remove != 0 ? true : false;
		}

        /// <summary>
        /// Проверить на корректность команды улучшения
        /// </summary>
        static bool IsCorrectCommand(string message, out string propety)
        {
            propety = "";
            var words = message.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (words.Length > 1)
            {                
                var array = new string[] { "perception", "reaction", "cunning", "boldness", "luck" };
                if (array.Any(s => s == words[1]))
                {
                    propety = words[1];
                    return true;
                }
            }    
            return false;
		}

        /// <summary>
        /// Создать пользователя
        /// </summary>
        public static void CreateUser(OnMessageReceivedArgs e, Bot bot)
        {
            using (var userContext = new AppDbContext())
            {
                bool isCreated = userContext.Get(e.ChatMessage.UserId) == null ? false : true;
                if (!isCreated)
                {
                    var user = GetNewUser(e.ChatMessage.UserId, e.ChatMessage.Username);
                    userContext.Users.Add(user);
                    //var car = CreateDefaultCarForUser(user);
                    //userContext.Cars.Add(car);
                    userContext.SaveChanges();

                    var message = @"@{0}, Теперь повышай свои характеристики и копи деньги, участвуя в тест драйвах";
                    bot.SendMessage(e.ChatMessage.Channel, string.Format(message, user.Login));
                }
            }
        }

        /// <summary>
        /// Создать пользователя с начальнами значениями
        /// </summary>
        static User GetNewUser(string id, string login)
        {
            var user = new User()
            {
                Id = id,
                Login = login,
                Money = 100,
                Experience = 100,
                RaceCount = 0,
                Victories = 0,
                Attentiveness = 1,
                SpeedReaction = 1,
                Сunning = 1,
                Сourage = 1,
                Luck = 1,
                RegistrationDate = DateTime.Now
            };
            return user;
        }

        /// <summary>
        /// Показать информацию о пользователе
        /// </summary>
        public static void ShowInfo(OnMessageReceivedArgs e, Bot bot)
        {
            User user;
            using (var userContext = new AppDbContext())
                user = userContext.Users.Where(x => x.Id == e.ChatMessage.UserId).FirstOrDefault();

            if (user != null)
            {
                var info = "@{0}, Гонок - {1}({2}) Внимательность - {3}, Скорость реакции {4}, " +
                                "Смелость - {5}, Хитрость - {6}, Удача - {7}, Опыт - {8}, Деньги - {9}";
                info = string.Format(info, user.Login, user.RaceCount, user.Victories, user.Attentiveness, user.SpeedReaction,
                                     user.Сourage, user.Сunning, user.Luck, user.Experience, user.Money);
                bot.SendMessage(e.ChatMessage.Channel, info);
            }
        }
    }
}
