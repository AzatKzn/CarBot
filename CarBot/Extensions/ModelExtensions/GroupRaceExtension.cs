using CarBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarBot.Extensions.ModelExtensions
{
	static class GroupRaceExtension
	{
		public static void AddParticipant(this GroupRace groupRace, User user)
		{
			groupRace.Participants.Add(user);
		}
	}
}
