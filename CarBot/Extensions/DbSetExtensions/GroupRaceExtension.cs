using CarBot.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CarBot.DbSetExtensions
{
	static class GroupRaceExtensions
	{
		public static GroupRace CreateAndAddGroupRace(this DbSet<GroupRace> groupRaces)
		{
			var groupRace = new GroupRace() { Participants = new List<GroupRaceParticipant>(), CreateDate = DateTime.Now };
			groupRaces.Add(groupRace);
			return groupRace;
		}

		public static GroupRace Get(this DbSet<GroupRace> groupRaces, int id)
		{
			return groupRaces.Where(x => x.Id == id).FirstOrDefault();
		}
	}
}
