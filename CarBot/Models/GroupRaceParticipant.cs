using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarBot.Models
{	
	class GroupRaceParticipant
	{
		public virtual int Id { get; set; }

		public virtual GroupRace GroupRace { get; set; }

		public virtual User User { get; set; }
	}
}
