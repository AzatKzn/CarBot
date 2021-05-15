using CarBot.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CarBot.DbSetExtensions
{
	static class UserExtensions
	{
		/// <summary>
		/// Получить пользователя
		/// </summary>
		/// <param name="userId">Ид пользователя</param>
		/// <returns>Пользователь</returns>
		public static User GetUser(this DbSet<User> users, string userId)
		{
			return users.Where(x => x.Id == userId).FirstOrDefault();
		}
	}
}
