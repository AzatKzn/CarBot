using CarBot.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace CarBot.DbSetExtensions
{
	public static class GroupRaceExtensions
	{
		public static GroupRace CreateAndAddGroupRace(this DbSet<GroupRace> groupRaces, RaceDivision raceDivision)
		{
			var groupRace = new GroupRace
			{ 
				CreateDate = DateTime.Now,
				RaceDivision = raceDivision
			};

			groupRaces.Add(groupRace);
			return groupRace;
		}

		public static GroupRace GetNotFinished(this DbSet<GroupRace> groupRaces)
		{
			return groupRaces.Where(x => x.IsFinished.HasValue && !x.IsFinished.Value).FirstOrDefault();
		}
	}
}
