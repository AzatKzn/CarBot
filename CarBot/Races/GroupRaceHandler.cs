using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client.Events;
using CarBot.Models;
using CarBot.DBContexts;
using CarBot.DbSetExtensions;

namespace CarBot.Races
{
	static class GroupRaceHandler
	{
		private class Reward
		{
			public int Score { get; set; }
			public int Money { get; set; }
			public int Experience { get; set; }
		}

		private class UserRaceResult
		{
			public User User { get; set; }
			public int Speed { get; set; }
		}

		static int CurrentRaceId = 0;
		public static void CreateRace(string channel, Bot bot)
		{
			using (var context = new AppDbContext())
			{
				var groupRace = context.GroupRaces.CreateAndAddGroupRace();
				CurrentRaceId = groupRace.Id;
				context.SaveChanges();
			}
			GroupRaceInfo(channel, bot);
		}

		public static void GroupRaceInfo(string channel, Bot bot)
		{
			if (CurrentRaceId <= 0) return;
			using (var context = new AppDbContext())
			{
				var groupRace = context.GroupRaces.Get(CurrentRaceId);
				if (groupRace == null) return;
				var message = string.Format("Через {0} минут стартует группой заезд!!! Для участия в гонке !join (необходима тачка), " +
									"текущее количество участников - {1}.", 5, groupRace.Participants.Count);
				bot.SendMessage(channel, message);
			}
		}

		public static void GroupRaceResult(Bot bot)
		{
			using (var context = new AppDbContext())
			{
				var groupRace = context.GroupRaces.Get(CurrentRaceId);
				CurrentRaceId = 0;
				var participents = groupRace.Participants;

				
				var userCars = participents.Select(x => context.Cars.GetUserCar(x.User)).ToList();
				var userResults = participents.Select(x => GetRaceResult(x, userCars.FirstOrDefault(x => x.User == x.User))).OrderByDescending(x => x.Speed).ToList();
				userCars.ForEach(x => x.Strength--);
				UpdateUsers(userResults);
				context.SaveChanges();
				var message = string.Format("Через {0} минут стартует группой заезд!!! Для участия в гонке !join (необходима тачка), " +
									"текущее количество участников - {1}.", 5, groupRace.Participants.Count);
				bot.SendMessage(Config.Channel, message);
			}
		}

		static void UpdateUsers(List<UserRaceResult> raceResults)
		{
			var count = raceResults.Count;
			raceResults.ForEach(x =>
			{
				var reward = GetReward(count, raceResults.IndexOf(x) + 1);
				x.User.Experience += reward.Experience;
				x.User.Money += reward.Money;
				x.User.ScoreInGroupRace += reward.Score;
			});
		}

		static Reward GetReward(int participentsCount, int place)
		{
			var reward = new Reward();
			return reward;
		}

		static UserRaceResult GetRaceResult(GroupRaceParticipant raceParticipant, UserCar userCar)
		{
			var user = raceParticipant.User;
			return new UserRaceResult() { User = user, Speed = GetSpeed(user, userCar) };
		}

		static int GetSpeed(User user, UserCar userCar)
		{
			var car = userCar.Auto;
			var random = new Random();
			var a1 = random.Next(user.Attentiveness, user.Attentiveness + user.Luck) * car.Speed;
			var a2 = random.Next(user.SpeedReaction, user.SpeedReaction + user.Luck) * car.Mobility;
			var a3 = random.Next(user.Сunning, user.Сunning + user.Luck) * car.Overclocking;
			var a4 = random.Next(user.Сourage, user.Сourage + user.Luck) * car.Braking;
			return a1 + a2 + a3 + a4;
		}

		public static void JoinToRace(OnMessageReceivedArgs e, Bot bot)
		{
			if (CurrentRaceId <= 0) return;
			using (var context = new AppDbContext())
			{
				var user = context.Users.Get(e.ChatMessage.UserId);
				if (user == null) return;
				var userCar = context.Cars.GetUserCar(user);
				if (userCar == null) return;
				var groupRace = context.GroupRaces.Get(CurrentRaceId);
				if (groupRace == null) return;
				groupRace.AddParticipant(user);
				context.SaveChanges();
			}
		}
	}	
}
