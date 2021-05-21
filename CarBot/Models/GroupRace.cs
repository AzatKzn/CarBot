using System;
using System.Collections.Generic;

namespace CarBot.Models
{
	class GroupRace
	{
		public virtual int Id { get; set; }

		public virtual List<GroupRaceParticipant> Participants { get; set;}

		public virtual DateTime CreateDate { get; set; }

		public void AddParticipant(User user)
		{
			this.Participants.Add(new GroupRaceParticipant() { GroupRace = this, User = user } );
		}
	}
}
