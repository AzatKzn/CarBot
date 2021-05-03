using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarBot.Models
{
	class UserCar
	{
		public int Id { get; set; }

		/// <summary>
		/// Владельца машины
		/// </summary>
		public User User { get; set; }

		/// <summary>
		/// Название машины
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Скорость
		/// </summary>
		public int Speed { get; set; }

		/// <summary>
		/// Маневренность 
		/// </summary>
		public int Mobility { get; set; }

		/// <summary>
		/// Разгон
		/// </summary>
		public int Overclocking { get; set; }

		/// <summary>
		/// Торможение
		/// </summary>
		public int Braking { get; set; }

		/// <summary>
		/// Прочность
		/// </summary>
		public int Strength { get; set; }
	}
}
