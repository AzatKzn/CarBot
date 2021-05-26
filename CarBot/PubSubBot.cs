using CarBot.DBContexts;
using CarBot.Models;
using System;
using System.Threading.Tasks;
using TwitchLib.Communication.Enums;
using TwitchLib.Communication.Models;
using TwitchLib.PubSub;
using TwitchLib.PubSub.Events;
using CarBot.DbSetExtensions;
using CarBot.BaseTypesExtensions;

namespace CarBot
{
	class PubSubBot
	{
		const string CantParseMessage = "Can`t parse reward - {0}(GUID - {1}, Cost - {2}, User - {3}, {4}).";
		private readonly TwitchPubSub client;
		public PubSubBot()
		{
			client = new TwitchPubSub();
			client.OnPubSubServiceConnected += onPubSubServiceConnected;
			client.OnPubSubServiceClosed += onPubSubServiceDisConnected;
			client.OnRewardRedeemed += OnRewardRedeemed;
			client.ListenToRewards("142647519");
			client.Connect();
		}

		private void OnRewardRedeemed(object sender, OnRewardRedeemedArgs e)
		{
			CheckReward(e);
		}

		private async void CheckReward(OnRewardRedeemedArgs e)
		{
			await Task.Run(() =>
			{
				try
				{
					if (e.RewardTitle.Contains("10к опыта в игре") || e.RewardId.ToString().ToLower() == "b2a275fe-b37e-4d69-bde4-9d59f0c39b7e")
						Upgrade(e, true, 10000);
					else if (e.RewardTitle.Contains("30к опыта в игре") || e.RewardId.ToString().ToLower() == "36827cd8-001d-4735-9b1e-7dbaf9604b2f")
						Upgrade(e, true, 30000);
					else if (e.RewardTitle.Contains("70к опыта в игре") || e.RewardId.ToString().ToLower() == "8fd25e79-1f1d-4d01-9096-df0e372e9cc1")
						Upgrade(e, true, 70000);
					else if (e.RewardTitle.Contains("10к денег в игре") || e.RewardId.ToString().ToLower() == "30db925c-97dc-4a8b-b9ab-570888095aef")
						Upgrade(e, false, 10000);
					else if (e.RewardTitle.Contains("30к денег в игре") || e.RewardId.ToString().ToLower() == "918d3bb0-7dc1-4e20-92d2-f5ee9a2c1c3e")
						Upgrade(e, false, 30000);
					else if (e.RewardTitle.Contains("70к денег в игре") || e.RewardId.ToString().ToLower() == "fe8df898-1f48-474c-bce4-334c707f9e72")
						Upgrade(e, false, 70000);
					else if(e.RewardCost >= 10000)
						LogInfo(e);
				}
				catch (Exception ex)
				{
					Logger.LogRewardError(ex);
					LogInfo(e);
				}
			});
		}

		private void LogInfo(OnRewardRedeemedArgs e)
		{
			var str = CantParseMessage.Format(e.RewardTitle, e.RewardId, e.RewardCost, e.Login, e.DisplayName);
			Logger.LogRewardInfo(str);
		}

		private void Upgrade(OnRewardRedeemedArgs e, bool isExp, int sum)
		{
			using (var context = new AppDbContext())
			{
				var user = context.Users.GetByLogin(e.Login);
				if (user == null)
				{
					LogInfo(e);
					return;
				}
				if (isExp)
					user.Experience += sum;
				else
					user.Money += sum;
				var rewardHistory = new RewardHistory()
				{
					User = user,
					Name = e.RewardTitle,
					Guid = e.RewardId,
					Cost = e.RewardCost,
					Date = DateTime.Now,
					FactUp = sum,
					IsExp = isExp
				};
				context.RewardHistories.Add(rewardHistory);
				context.SaveChanges();

			}
		}

		private void onPubSubServiceDisConnected(object sender, EventArgs e)
		{
			try
			{
				Logger.LogRewardInfo("Disconnected.");
				Logger.LogRewardInfo("Try to reconnect...");
				client.Connect();
				Logger.LogRewardInfo("Reconnected...");
			}
			catch (Exception ex)
			{
				Logger.LogRewardError(ex);
			}
		}

		private void onPubSubServiceConnected(object sender, EventArgs e)
		{
			client.SendTopics(Config.OAuthPubSub);
			Logger.LogRewardInfo("Connected to reward channel for {0}".Format(Config.Channel));
		}
	}
}