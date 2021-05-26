using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client.Events;
using CarBot.Models;
using CarBot.DBContexts;
using CarBot.DbSetExtensions;
using CarBot.BaseTypesExtensions;
using System.Threading;
using Microsoft.EntityFrameworkCore;

namespace CarBot.Races
{
	static class GroupRaceHandler
	{
		private class Reward
		{
			public double Percent { get; set; }
			public int Money { get; set; }
			public int Experience { get; set; }
			public int Place { get; set; }
		}

		private class UserRaceResult
		{
			public User User { get; set; }
			public Reward Reward { get; set; }
			public int Speed { get; set; }
		}

		static string url = new Uri(new Uri(Config.Url), "grouprace/result/").ToString();

		/// <summary>
		/// Создать гонку
		/// </summary>
		public static void CreateRace(string channel, Bot bot, RaceDivision raceDivision)
		{
			using (var context = new AppDbContext())
			{
				var groupRace = context.GroupRaces.CreateAndAddGroupRace(raceDivision);
				context.SaveChanges();
			}
			GroupRaceInfo(channel, bot, 10);
		}

		/// <summary>
		/// Вывести сообщение о гонке
		/// </summary>
		public static void GroupRaceInfo(string channel, Bot bot, int minutesLeft)
		{
			using (var context = new AppDbContext())
			{
				var groupRace = context.GroupRaces.GetNotFinished();
				if (groupRace == null) return;
				string uri = url + groupRace.Id.ToString();
				var groupRaceParticipant = context.GroupRaceParticipant.Where(x => x.GroupRace.Id == groupRace.Id).ToList();
				var message = "Через {0} минут стартует группой заезд, дивизион: {2}. Для вступления команда !join. Текущее количество участников: {3}, {1}".Format(minutesLeft, uri,
																								groupRace.RaceDivision.GetDisplayName(), groupRaceParticipant.Count);
				bot.SendMessage(channel, message);
			}
		}

		/// <summary>
		/// Расчет результатов гонки и наград.
		/// </summary>
		public static void GroupRaceResult(string channel, Bot bot)
		{
			using (var context = new AppDbContext())
			{
				var groupRace = context.GroupRaces.GetNotFinished();
				groupRace.IsFinished = true;
				context.SaveChanges();
				var groupRaceParticipants = context.GroupRaceParticipant
					.Where(x => x.GroupRace.Id == groupRace.Id)
					.Include(x => x.User)
					.Include(x => x.UserCar).ThenInclude(x => x.Auto)
					.ToList();
				if (groupRaceParticipants.Count < 4)
				{
					bot.SendMessage(channel, "Гонка отменена. Недостаточно участников (необходимо минимум 3).");
					var returnMoney = groupRace.RaceDivision.GetJoinCost();
					groupRaceParticipants.ForEach(x => x.User.Money += returnMoney);
					context.SaveChanges();
					return;
				}

				var message = "Групповой заезд стартовал!!!";
				bot.SendMessage(channel, message);
				Thread.Sleep(TimeSpan.FromMinutes(1));

				var participents = groupRaceParticipants;

				var userResults = participents
					.Select(x => GetRaceResult(x))
					.OrderByDescending(x => x.Speed)
					.ToList();

				UpdateUsers(userResults, groupRace.RaceDivision);

				participents.ForEach(x =>
				{
					var result = userResults.Where(y => y.User.Id == x.User.Id).First();

					x.Score = result.Speed;
					x.Money = result.Reward.Money;
					x.Experience = result.Reward.Experience;
					x.Place = result.Reward.Place;
					x.UserCar.Strength--;
					x.User.GroupRaceCount++;
					x.User.Experience += result.Reward.Experience;
					x.User.Money += result.Reward.Money;
					x.User.ScoreInGroupRace += result.Speed;
				});
				var first = participents.Where(x => x.Place == 1).FirstOrDefault();
				first.User.GroupRaceVictories++;
				context.SaveChanges();
				message = "Результаты гонки: {0} . Победитель - @{1}.".Format(url + groupRace.Id.ToString(), first.User.Login);
				bot.SendMessage(channel, message);
			}
		}

		/// <summary>
		/// Расчет награды для участников
		/// </summary>
		static void UpdateUsers(List<UserRaceResult> raceResults, RaceDivision raceDivision)
		{
			var count = raceResults.Count;
			var fond = count * raceDivision.GetJoinCost();
			raceResults.ForEach(x =>
			{
				var place = raceResults.IndexOf(x) + 1;
				var reward = GetReward(count, place);
				x.Reward = reward;
				x.Reward.Place = place;
				x.Reward.Money = (int)(fond * reward.Percent);
				x.Reward.Experience = new Random().Next(3000 + x.User.Luck * 30, 3100 + x.User.Luck * 50);
			});
		}

		/// <summary>
		/// Получить процент выйгрыша от призового фонда для участника по его месту и общему количеству участников
		/// </summary>
		static Reward GetReward(int participentsCount, int place)
		{
			var reward = new Reward();

			#region 4-5 участника  распределяется до 95% фонда
			if (participentsCount == 4 || participentsCount == 5)
			{
				if (place == 1)
				{
					reward.Percent = 0.35;
				}
				else if (place == 2)
				{
					reward.Percent = 0.25;
				}
				else if (place == 3)
				{
					reward.Percent = 0.2;
				}
				else if (place == 4)
				{
					reward.Percent = 0.1;
				}
				else if (place == 5)
				{
					reward.Percent = 0.05;
				}
			}
			#endregion

			#region 6-7 участника распределяется до 95% фонда
			else if (participentsCount == 6 || participentsCount == 7)
			{
				if (place == 1)
				{
					reward.Percent = 0.33;
				}
				else if (place == 2)
				{
					reward.Percent = 0.22;
				}
				else if (place == 3)
				{
					reward.Percent = 0.18;
				}
				else if (place == 4)
				{
					reward.Percent = 0.12;
				}
				else if (place == 5)
				{
					reward.Percent = 0.08;
				}
				else if (place == 6)
				{
					reward.Percent = 0.05;
				}
				else if (place == 7)
				{
					reward.Percent = 0.02;
				}
			}
			#endregion

			#region 8-9 участника распределяется до 95% фонда
			else if (participentsCount == 8 || participentsCount == 9)
			{
				if (place == 1)
				{
					reward.Percent = 0.25;
				}
				else if (place == 2)
				{
					reward.Percent = 0.20;
				}
				else if (place == 3)
				{
					reward.Percent = 0.15;
				}
				else if (place == 4)
				{
					reward.Percent = 0.14;
				}
				else if (place == 5)
				{
					reward.Percent = 0.11;
				}
				else if (place == 6)
				{
					reward.Percent = 0.08;
				}
				else if (place == 7)
				{
					reward.Percent = 0.06;
				}
				else if (place == 8)
				{
					reward.Percent = 0.04;
				}
				else if (place == 9)
				{
					reward.Percent = 0.02;
				}
			}
			#endregion

			#region больше 10 участников распределяется до 95% фонда
			else if (participentsCount >= 10)
			{
				if (place == 1)
				{
					reward.Percent = 0.20;
				}
				else if (place == 2)
				{
					reward.Percent = 0.17;
				}
				else if (place == 3)
				{
					reward.Percent = 0.14;
				}
				else if (place == 4)
				{
					reward.Percent = 0.12;
				}
				else if (place == 5)
				{
					reward.Percent = 0.08;
				}
				else if (place == 6)
				{
					reward.Percent = 0.08;
				}
				else if (place == 7)
				{
					reward.Percent = 0.05;
				}
				else if (place == 8)
				{
					reward.Percent = 0.05;
				}
				else if (place == 9)
				{
					reward.Percent = 0.03;
				}
				else if (place == 10)
					reward.Percent = 0.03;
				else
				{
					reward.Percent = 0;
				}
			}
			#endregion

			return reward;
		}

		/// <summary>
		/// Рассчитать результат участника гонки 
		/// </summary>
		static UserRaceResult GetRaceResult(GroupRaceParticipant raceParticipant)
		{
			var user = raceParticipant.User;
			UserCar userCar = raceParticipant.UserCar;
			return new UserRaceResult() { User = user, Speed = GetSpeed(user, userCar) };
		}

		/// <summary>
		/// Получить скорость
		/// </summary>
		static int GetSpeed(User user, UserCar userCar)
		{
			var car = userCar.Auto;
			var random = new Random();
			var a1 = random.Next(user.Attentiveness, user.Attentiveness + user.Luck) * car.Speed;
			var a2 = random.Next(user.SpeedReaction, user.SpeedReaction + user.Luck) * car.Mobility;
			var a3 = random.Next(user.Cunning, user.Cunning + user.Luck) * car.Overclocking;
			var a4 = random.Next(user.Courage, user.Courage + user.Luck) * car.Braking;
			return a1 + a2 + a3 + a4;
		}

		/// <summary>
		/// Добавление участника в гону
		/// </summary>
		public static void JoinToRace(OnMessageReceivedArgs e, Bot bot)
		{
			using (var context = new AppDbContext())
			{
				var user = context.Users.Get(e.ChatMessage.UserId);
				if (user == null) return;

				var userCar = context.Cars.GetUserCar(user);
				if (userCar == null) return;

				var groupRace = context.GroupRaces.GetNotFinished();
				if (groupRace == null) return;

				if (context.GroupRaceParticipant.Any(x => x.GroupRace.Id == groupRace.Id && x.User.Id == user.Id))
					return;
				if (!groupRace.RaceDivision.CanJoin(user))
					return;
				user.Login = e.ChatMessage.Username;
				user.Money -= groupRace.RaceDivision.GetJoinCost();
				var s = new GroupRaceParticipant() { GroupRace = groupRace, User = user, UserCar = userCar };
				context.GroupRaceParticipant.Add(s);
				context.SaveChanges();
			}
		}
	}
}
