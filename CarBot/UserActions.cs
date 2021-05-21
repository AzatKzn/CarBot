using CarBot.DBContexts;
using CarBot.DbSetExtensions;
using CarBot.Models;
using System;
using System.Linq;
using TwitchLib.Client.Events;
using CarBot.BaseTypesExtensions;

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
        public static void ShowUserCar(OnMessageReceivedArgs e, Bot bot)
        {
            using (var context = new AppDbContext())
            {
                var user = context.Users.Get(e.ChatMessage.UserId);
                if (user == null)
                    return;
                var userCar = context.Cars.GetUserCar(user);
                if (userCar == null)
                    return;
                var auto = userCar.Auto;
                var message = "@{0}, твоя тачка: {1}, скорость - {2}, маневренность - {3}, разгон - {4}, торможение - {5}, прочность - {6}%.".Format(user.Login,
                                            auto.Name, auto.Speed, auto.Mobility, auto.Overclocking, auto.Braking, userCar.Strength.ToString("0.0"));
                bot.SendMessage(e.ChatMessage.Channel, message);
            }
		}

        /// <summary>
        /// Повышение характеристики
        /// </summary>
        /// <remarks>Message начинается с !lvlup и должна содержать имя повышаемой хар-ки</remarks>
        public static void Upgrade(OnMessageReceivedArgs e, Bot bot)
        {            
            using (var context = new AppDbContext())
            {
                var user = context.Users.Get(e.ChatMessage.UserId);
                string propety;
                var message = "";
                UpgradeResult result; 
                if (user != null && IsCorrectCommand(e.ChatMessage.Message.ToLower(), out propety))
                {
                    string propertyRu;
                    result = TryUpgrade(user, propety, out propertyRu);
                    context.SaveChanges();
                    if (result.IsSuccess)
                    {
                        message = "@{0}, ваша характеристика \"{1}\" увеличена до {2}.".Format(e.ChatMessage.Username, propertyRu, result.NewLVL);
                    }
                    else if (!result.IsSuccess && result.NeedExp != 0)
                    {
                        message = "@{0}, для повышения характеристики \"{1}\" не хватает {2} опыта.".Format(e.ChatMessage.Username, propertyRu, result.NeedExp);
                    }
                    else if (!result.IsSuccess && result.NewLVL == -1)
                        message = "@{0}, ваша характеристика \"{1}\" максимального уровня.".Format(e.ChatMessage.Username, propertyRu);
                    bot.SendMessage(e.ChatMessage.Channel, message);
                }
                
            }
        }

        static UpgradeResult TryUpgrade(User user, string upgradePropety, out string propertyRu)
        {
            int remove = 0;
            bool isCanUpdate = false;
            int newLVL = 0;
            propertyRu = string.Empty;
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
                        propertyRu = "внимательность";
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
                        propertyRu = "скорость реакции";
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
                        propertyRu = "хитрость";
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
                        propertyRu = "смелость";
                        break;
                    }
                case "luck":
                    {
                        isCanUpdate = IsEnoughMoney(user.Experience, user.Luck, out remove, true);
                        if (isCanUpdate)
                        {
                            user.Experience -= remove;
                            user.Luck++;
                            newLVL = user.Luck;                            
                        }
                        propertyRu = "удача";
                        break;
                    }
            }
            int needExp = 0;
            if (!isCanUpdate && remove == 0)
                return new UpgradeResult() { IsSuccess = false, NewLVL = -1 };
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
        static bool IsEnoughMoney(long exp, int currentLVL, out int remove, bool IsLuckProperty = false)
        {
            remove = 0;
            if (!IsLuckProperty)
            {
                if (currentLVL == 99)
                    return false;
                if (currentLVL <= 9)
                    remove = Config.LVLCost[currentLVL];
                else if (currentLVL > 9)
                    remove = Config.LVLCost[10];
            }
            else 
            {
                if (currentLVL == 7)
                    return false;
                remove = currentLVL * 10000;                
            }
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
            using (var context = new AppDbContext())
            {
                bool isCreated = context.Users.Get(e.ChatMessage.UserId) == null ? false : true;
                if (!isCreated)
                {
                    var user = GetNewUser(e.ChatMessage.UserId, e.ChatMessage.Username);
                    context.Users.Add(user);
                    context.SaveChanges();

                    var message = "@{0}, Теперь повышай свои характеристики и копи деньги, участвуя в тест драйвах";
                    bot.SendMessage(e.ChatMessage.Channel, message.Format(e.ChatMessage.Username));
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
                var info = "@{0}, Гонок c ИИ - {1}({2}), Тест драйвов - {10}, Внимательность - {3}, Скорость реакции {4}, " +
                                "Смелость - {5}, Хитрость - {6}, Удача - {7}, Опыт - {8}, Деньги - {9}";
                info = info.Format(user.Login, user.RaceCountWithAI, user.VictoriesWithAI, user.Attentiveness, user.SpeedReaction,
                                     user.Сourage, user.Сunning, user.Luck, user.Experience, user.Money, user.TestDrivesCount);
                bot.SendMessage(e.ChatMessage.Channel, info);
            }
        }
    }
}
