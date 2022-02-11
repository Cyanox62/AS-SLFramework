using Exiled.API.Features;
using HarmonyLib;

namespace ServerStatistics.Patches
{
    [HarmonyPatch(typeof(RemoteAdmin.CommandProcessor), nameof(RemoteAdmin.CommandProcessor.ProcessQuery))]
    static class RemoteAdminQuery
    {
        public static void Prefix(string q, CommandSender sender)
		{
            Player player = Player.Get(sender);
            if (player != null)
			{
				Utils.HandleRACommand(player, q);
				if (!q.Contains("PLAYER_LIST SILENT"))
				{
					using (dWebHook dcWeb = new dWebHook())
					{
						dcWeb.ProfilePicture = Plugin.singleton.Config.CommandLogAvatarURL;
						dcWeb.UserName = Plugin.singleton.Config.CommandLogName;
						dcWeb.WebHook = Plugin.singleton.Config.CommandLogWebhook;
						dcWeb.SendMessage(Plugin.singleton.Translation.CommandMessage
							.Replace("{commandSender}", player.Nickname)
							.Replace("{commandUserid}", player.UserId)
							.Replace("{command}", q));
					}
				}
			}
		}
	}
}
