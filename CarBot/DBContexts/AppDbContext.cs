using CarBot.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace CarBot.DBContexts
{
	public class AppDbContext : DbContext
	{
		public DbSet<User> Users { get; set; }

		public DbSet<History> Histories { get; set; }

		public DbSet<UserCar> Cars { get; set; }

		public DbSet<Auto> Autos { get; set; }

		public DbSet<GroupRaceParticipant> GroupRaceParticipant { get; set; }

		public DbSet<GroupRace> GroupRaces { get; set; }

		public DbSet<RewardHistory> RewardHistories { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			Config.LoadConfig();
			
			optionsBuilder.UseMySql(
				Config.ConnectionString, 
				new MySqlServerVersion(new Version(5, 7, 21)));
		}
	}
}
