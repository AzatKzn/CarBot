using CarBot.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace CarBot.DbSetExtensions
{
	static class HistoryExtensions
	{
		/// <summary>
		/// Получить последнюю историю действий пользователя с датой
		/// </summary>
		/// <param name="user">Пользователь</param>
		/// <param name="actionType">Тип действия</param>
		/// <returns>История</returns>
		public static History GetLastUserHistory(this DbSet<History> histories, User user, ActionType actionType)
		{
			return histories.Where(x => x.User.Id == user.Id && x.ActionType == actionType).OrderByDescending(x => x.Date).FirstOrDefault();
		}

		public static void CreateAndAddHistory(this DbSet<History> histories, User user, ActionType actionType, UserCar car = null)
		{
			histories.Add(new History() { User = user, ActionType = actionType, Date = DateTime.Now, Car = car });
		}
	}
}
