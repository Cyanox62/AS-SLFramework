using Exiled.API.Features;
using System.Collections.Generic;
using System.Linq;

namespace ServerStatistics
{
	internal static class Utils
	{
		private static int[] div = { 60, 24, 30, 12 };
		private static string[] suffix = { "m", "h", "d", "mon", "y" };

		internal static void HandleRACommand(Player sender, string query)
		{
			query = query.ToLower();
			string[] tempArgs = query.Split(' ');
			string cmd = tempArgs[0];
			List<string> arguments = tempArgs.Skip(1).ToList();
			if (cmd == "ban")
			{
				if (int.TryParse(arguments[0].Replace(".", "").Trim(), out int pid))
				{
					Player player = Player.Get(pid);

					if (int.TryParse(arguments[1].Trim(), out int t))
					{
						if (t == 0)
						{
							using (dWebHook dcWeb = new dWebHook())
							{
								dcWeb.ProfilePicture = Plugin.singleton.Config.BanLogAvatarURL;
								dcWeb.UserName = Plugin.singleton.Config.BanLogName;
								dcWeb.WebHook = Plugin.singleton.Config.BanLogWebhook;
								dcWeb.SendMessage(Plugin.singleton.Translation.KickMessage
									.Replace("{targetNickname}", player.Nickname)
									.Replace("{targetUserid}", player.UserId)
									.Replace("{senderNickname}", sender.Nickname)
									.Replace("{senderUserid}", sender.UserId));
							}
						}
						else
						{
							int depth = 0;
							int time = t;
							while (t > 1)
							{
								time = t;
								t /= div[depth];
								if (t > 1) depth++;
							}

							using (dWebHook dcWeb = new dWebHook())
							{
								dcWeb.ProfilePicture = Plugin.singleton.Config.BanLogAvatarURL;
								dcWeb.UserName = Plugin.singleton.Config.BanLogName;
								dcWeb.WebHook = Plugin.singleton.Config.BanLogWebhook;
								dcWeb.SendMessage(Plugin.singleton.Translation.BanMessage
									.Replace("{targetNickname}", player.Nickname)
									.Replace("{targetUserid}", player.UserId)
									.Replace("{senderNickname}", sender.Nickname)
									.Replace("{senderUserid}", sender.UserId)
									.Replace("{time}", time + suffix[depth]));
							}
						}
					}
				}
			}
			else if (cmd == "mute")
			{
				if (int.TryParse(arguments[0].Replace(".", "").Trim(), out int pid))
				{
					Player player = Player.Get(pid);

					using (dWebHook dcWeb = new dWebHook())
					{
						dcWeb.ProfilePicture = Plugin.singleton.Config.BanLogAvatarURL;
						dcWeb.UserName = Plugin.singleton.Config.BanLogName;
						dcWeb.WebHook = Plugin.singleton.Config.BanLogWebhook;
						dcWeb.SendMessage(Plugin.singleton.Translation.MuteMessage
							.Replace("{targetNickname}", player.Nickname)
							.Replace("{targetUserid}", player.UserId)
							.Replace("{senderNickname}", sender.Nickname)
							.Replace("{senderUserid}", sender.UserId));

					}
				}
			}
		}
	}
}
