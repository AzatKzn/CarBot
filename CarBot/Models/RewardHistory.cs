using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarBot.Models
{
	class RewardHistory
	{
		public virtual int Id { get; set; }

		public virtual User User { get; set; }

		public virtual string Name { get; set; }

		public virtual Guid Guid { get; set; }

		public virtual int Cost { get; set; }

		public virtual DateTime Date { get; set; }

		public virtual int FactUp { get; set; }

		public virtual bool IsExp { get; set; }
	}
}
