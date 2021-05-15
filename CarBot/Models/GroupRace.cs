using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarBot.Models
{
	class GroupRace
	{
		public int Id { get; set; }

		public virtual List<User> Participants { get; set;}

		public DateTime CreateDate { get; set; }
	}
}
