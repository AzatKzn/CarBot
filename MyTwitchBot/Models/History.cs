using System;

namespace CarBot.Models
{
	public enum ActionType
	{
		TestDrive = 10,
		RaceWithAI = 20,
	}

	class History
	{
		public int Id { get; set; }

		public User User { get; set; }

		public UserCar Car { get; set; }

		public ActionType? ActionType { get; set; }

		public DateTime Date { get; set; }
	}
}
