using CarBot.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CarBot.DbSetExtensions
{
	static class UserCarExtensions
	{
		/// <summary>
		/// Получить машину пользователя
		/// </summary>
		/// <param name="user">Пользователь</param>
		/// <returns>Машина</returns>
		public static UserCar GetUserCar(this DbSet<UserCar> cars, User user)
		{
			return cars.Where(x => x.User.Id.Equals(user.Id) && x.IsActive).Include(x => x.Auto).OrderByDescending(x => x.BuyDate).FirstOrDefault();
		}
	}
}
