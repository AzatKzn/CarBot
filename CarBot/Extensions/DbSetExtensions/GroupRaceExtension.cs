using CarBot.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace CarBot.DbSetExtensions
{
	static class GroupRaceExtensions
	{
		public static void CreateAndSaveGroupRace(this DbSet<GroupRace> groupRaces)
		{
			groupRaces.Add(new GroupRace() { Participants = new List<User>(), CreateDate = DateTime.Now });
		}
	}
}
