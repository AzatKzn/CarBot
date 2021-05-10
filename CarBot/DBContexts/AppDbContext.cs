using Microsoft.EntityFrameworkCore;
using CarBot.Models;
using System;
using System.Linq;

namespace CarBot.DBContexts
{
	class AppDbContext : DbContext
	{
		public DbSet<User> Users { get; set; }

		public DbSet<History> Histories { get; set; }

		public DbSet<UserCar> Cars { get; set; }

		public DbSet<Auto> Autos { get; set; }

		public History GetLastHistory(User user, ActionType actionType)
		{
			return Histories.Where(x => x.User.Id == user.Id && x.ActionType == actionType).OrderByDescending(x => x.Date).FirstOrDefault();
		}

		public UserCar GetUserCar(string userId)
		{
			return Cars.Where(x => x.User.Id.Equals(userId)).OrderByDescending(x => x.BuyDate).FirstOrDefault();
		}

		public User GetUser(string userId)
		{
			return Users.Where(x => x.Id == userId).FirstOrDefault();
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			Configuration.LoadConfig();

			optionsBuilder.UseMySql(
				Configuration.ConnectionString, 
				new MySqlServerVersion(new Version(5, 7, 21)));
		}
	}
}
