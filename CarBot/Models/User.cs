using System;

namespace CarBot.Models
{
	public class User
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
		public int GroupRaceCount { get; set; }

		/// <summary>
		/// Побед
		/// </summary>
		public int GroupRaceVictories { get; set; }

		/// <summary>
		/// Количество гонок с ИИ
		/// </summary>
		public int RaceCountWithAI { get; set; }

		/// <summary>
		/// Побед с ИИ
		/// </summary>
		public int VictoriesWithAI { get; set; }

		/// <summary>
		/// Побед с ИИ Easy
		/// </summary>
		public int VictoriesWithAIEasy { get; set; }

		/// <summary>
		/// Побед с ИИ Normal
		/// </summary>
		public int VictoriesWithAINormal { get; set; }

		/// <summary>
		/// Побед с ИИ Hard
		/// </summary>
		public int VictoriesWithAIHard { get; set; }

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
		public int Cunning { get; set; }

		/// <summary>
		/// Смелость
		/// </summary>
		public int Courage { get; set; }

		/// <summary>
		/// Удача
		/// </summary>
		public int Luck { get; set; }

		/// <summary>
		/// Дата регистрации
		/// </summary>
		public DateTime RegistrationDate { get; set; }

		/// <summary>
		/// Количество тест драйвов
		/// </summary>
		public int TestDrivesCount { get; set; }

		/// <summary>
		/// Очки в групповых заездах
		/// </summary>
		public int ScoreInGroupRace { get; set; }
	}
}
