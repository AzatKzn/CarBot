using CarBot.DBContexts;
using System;
using System.Collections.Generic;

namespace CarBot.Models
{
	public class GroupRace
	{
		public virtual int Id { get; set; }

		public virtual DateTime CreateDate { get; set; }

		public virtual string Hash { get; set; }

		public virtual bool? IsFinished { get; set; }

		public virtual RaceDivision RaceDivision { get; set; }

		const string str = "abcdefghijklmnopqrstuvwxyz123456789abcdefghijklmnopqrstuvwxyz123456789abcdefghijklmnopqrstuvwxyz123456789abcdefghijklmnopqrstuvwxyz123456789";

		public GroupRace()
		{
			Random random = new Random();
			IsFinished = false;
			Hash = string.Empty;
			while (Hash.Length < 15)
				Hash += str[random.Next(str.Length)]; 
		}

		
	}

	public enum RaceDivision 
	{
		Newbie = 0,
		Common = 10,
		Pro = 20,
	}

	public static class RaceDivisionExtensions
	{
		public static string GetDisplayName(this RaceDivision raceDivision)
		{
			if (raceDivision == RaceDivision.Newbie)
				return "Новички";
			else if (raceDivision == RaceDivision.Common)
				return "Общий";
			else if (raceDivision == RaceDivision.Pro)
				return "Профи";
			return null;
		}

		public static bool CanJoin(this RaceDivision raceDivision, User user)
		{
			var count = user.Attentiveness + user.SpeedReaction + user.Cunning + user.Courage;

			if (user.Money < raceDivision.GetJoinCost())
				return false;

			if (raceDivision == RaceDivision.Common)
				return true;

			else if (raceDivision == RaceDivision.Newbie)
				return count < 40;

			else if (raceDivision == RaceDivision.Pro)
				return count >= 40;

			return false;
		}

		public static int GetJoinCost(this RaceDivision raceDivision)
		{
			if (raceDivision == RaceDivision.Common)
				return 1000;
			else if (raceDivision == RaceDivision.Newbie)
				return 1000;
			else if (raceDivision == RaceDivision.Pro)
				return 5000;
			return 0;
		}
	}
}
