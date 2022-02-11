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
				using (dWebHook dcWeb = new dWebHook())
				{
					dcWeb.ProfilePicture = Plugin.singleton.Config.CommandLogAvatarURL;
					dcWeb.UserName = Plugin.singleton.Config.BanLogName;
					dcWeb.WebHook = Plugin.singleton.Config.BanLogWebhook;
					dcWeb.SendMessage($":keyboard: [RA] {player.Nickname} ({player.UserId}) >: {q}");
				}
			}
		}
	}
}
