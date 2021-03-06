using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarBot.Models
{
	public class UserCar
	{

		public int Id { get; set; }

		public bool IsActive { get; set; }

		/// <summary>
		/// Владелец машины
		/// </summary>
		public User User { get; set; }

		/// <summary>
		/// Авто
		/// </summary>
		public Auto Auto { get; set; }

		/// <summary>
		/// Дата покупки
		/// </summary>
		public DateTime BuyDate { get; set; }

		/// <summary>
		/// Прочность
		/// </summary>
		public float Strength { get; set; }
	}
}
