using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CarBot.DBContexts;

namespace CarBot
{
	/// <summary>
	/// Класc для вознаграждения пользователей за просмотр стрима
	/// </summary>
	//static class UserActivity
	//{
	//	static List<string> lastUsers;
	//	readonly static string Channel = Configuration.Channel;
	//	public async static void CheckUserActivity()
	//	{
	//		await Task.Run(() =>
	//		{
	//			while (true)
	//			{
	//				lastUsers = GetUsers(Channel);
	//				Thread.Sleep(TimeSpan.FromMinutes(10));

	//				var currentUsers = GetUsers(Channel);

	//				var usersForUpdate = lastUsers.Where(u => currentUsers.Contains(u));

	//				UpdateUsers(usersForUpdate);
	//				// заполняем список
	//				// спим 10 минут
	//				// проверяем остались ли они
	//			}
	//		});
	//	}

	//	public async static void UpdateUsers(IEnumerable<string> userLogins)
	//	{
	//		await Task.Run(() =>
	//		{
	//			using (var context = new AppDbContext())
	//			{
	//				var users = context.Users.Where(x => userLogins.Contains(x.Login));
	//				foreach (var user in users)
	//					user.Money += 20;
	//				context.SaveChanges();
	//			}
	//		});
	//	}

	//	public static List<string> GetUsers(string channel)
	//	{
	//		var uri = string.Format("https://tmi.twitch.tv/group/user/{0}/chatters", channel);
	//		var request = WebRequest.Create(uri);
	//		var response = request.GetResponse();
	//		var result = new StringBuilder();
	//		using (var stream = response.GetResponseStream())
	//		{
	//			using (var reader = new StreamReader(stream))
	//			{
	//				var line = "";
	//				while ((line = reader.ReadLine()) != null)
	//					result.Append(line);
	//			}
	//		}
	//		response.Close();
	//		var str = result.ToString();
	//		var r = JsonConvert.DeserializeObject<Result>(str);
	//		var s = r.Chatters;
	//		var users = new List<string>();
	//		users.AddRange(s.Vips);;
	//		users.AddRange(s.Moderators);
	//		users.AddRange(s.Viewers);
	//		return users;
	//	}		
	//}

	//class Result
	//{
	//	public Chatters Chatters { get; set; }
	//}

	//class Chatters
	//{
	//	public List<string> Vips { get; set; }
	//	public List<string> Moderators { get; set; }
	//	public List<string> Viewers { get; set; }
	//}
}
