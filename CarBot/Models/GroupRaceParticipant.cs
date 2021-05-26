using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarBot.Models
{	
	public class GroupRaceParticipant
	{
		public virtual int Id { get; set; }

		public virtual GroupRace GroupRace { get; set; }

		public virtual User User { get; set; }

		public virtual UserCar UserCar { get; set; }

		public virtual int Score { get; set; }

		public virtual int Money { get; set; }

		public virtual int Experience { get; set; }

		public virtual int Place { get; set; }
	}
}
