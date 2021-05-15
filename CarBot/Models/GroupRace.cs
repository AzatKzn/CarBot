using System;
using System.Collections.Generic;

namespace CarBot.Models
{
	class GroupRace
	{
		public int Id { get; set; }

		public virtual List<User> Participants { get; set;}

		public DateTime CreateDate { get; set; }

		public void AddParticipant(User user)
		{
			this.Participants.Add(user);
		}
	}
}
