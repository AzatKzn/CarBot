﻿using System;

namespace CarBot.Models
{
	class User
	{
		/// <summary>
		/// Ид 
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		/// Логин 
		/// </summary>
		public string Login { get; set; }

		/// <summary>
		/// Деньги
		/// </summary>
		public long Money { get; set; }

		/// <summary>
		/// Опыт
		/// </summary>
		public long Experience { get; set; }

		/// <summary>
		/// Количество гонок
		/// </summary>
		public int RaceCount { get; set; }

		/// <summary>
		/// Побед
		/// </summary>
		public int Victories { get; set; }

		/// <summary>
		/// Внимательность
		/// </summary>
		public int Attentiveness { get; set; }

		/// <summary>
		/// Скорость реакции
		/// </summary>
		public int SpeedReaction { get; set; }

		/// <summary>
		/// Хитрость
		/// </summary>
		public int Сunning { get; set; }

		/// <summary>
		/// Смелость
		/// </summary>
		public int Сourage { get; set; }

		/// <summary>
		/// Удача
		/// </summary>
		public int Luck { get; set; }

		/// <summary>
		/// Дата регистрации
		/// </summary>
		public DateTime RegistrationDate { get; set; }

	}
}