using Exiled.API.Features;
using Exiled.API.Features.DamageHandlers;
using Exiled.Events.EventArgs;
using MEC;
using Newtonsoft.Json;
using PatreonPerks.Perks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PatreonPerks
{
	class EventHandlers
	{
		internal void OnRoundRestart()
		{
			File.WriteAllText(Plugin.GroupOverridesFile, JsonConvert.SerializeObject(Plugin.groups, Formatting.Indented));
			File.WriteAllText(Plugin.UserSettings, JsonConvert.SerializeObject(Plugin.userPerkSettings, Formatting.Indented, Plugin.userSerializeSettings));
		}

		internal void OnPlayerVerified(VerifiedEventArgs ev)
		{
			if (Plugin.groups.ContainsKey(ev.Player.UserId))
			{
				UserGroup group = ServerStatic.PermissionsHandler.GetGroup(Plugin.groups[ev.Player.UserId]);
				if (group != null)
				{
					ev.Player.Group = group;
				}
				else
				{
					Log.Warn($"Failed to assign group {Plugin.groups[ev.Player.UserId]}");
				}
			}

			if (Plugin.userPerkSettings.ContainsKey(ev.Player.UserId))
			{
				for (int i = Plugin.userPerkSettings[ev.Player.UserId].Count - 1; i >= 0; i--)
				{
					var perk = Plugin.userPerkSettings[ev.Player.UserId][i];
					if (!Plugin.perkLinks.ContainsKey(ev.Player.GroupName) ||
						(Plugin.perkLinks.ContainsKey(ev.Player.GroupName) &&
						!Plugin.perkLinks[ev.Player.GroupName].Select(x => x.Name).Contains(perk.PerkName)))
					{
						Plugin.userPerkSettings[ev.Player.UserId].RemoveAt(i);
					}
				}
			}

			Type t = typeof(AnnounceJoin);
			AnnounceJoin settings = (AnnounceJoin)Plugin.GetPerkSettings(ev.Player, t);
			if (Plugin.perkLinks.ContainsKey(ev.Player.GroupName) && Plugin.perkLinks[ev.Player.GroupName].Contains(t) && settings.Param != string.Empty)
			{
				Map.Broadcast(Plugin.singleton.Config.AnnounceJoinTime, Plugin.singleton.Translation.AnnounceJoin.Replace("{player}", ev.Player.Nickname));
			}
		}

		internal void OnAssignGroup(ChangingGroupEventArgs ev)
		{
			string groupName = Plugin.GetGroupName(ev.NewGroup);
			if (Plugin.perkLinks.ContainsKey(groupName))
			{
				if (!Plugin.userPerkSettings.ContainsKey(ev.Player.UserId))
				{
					Plugin.userPerkSettings.Add(ev.Player.UserId, new List<IPerk>());
				}
				IEnumerable<string> perks = Plugin.userPerkSettings[ev.Player.UserId].Select(x => x.PerkName);
				foreach (Type perk in Plugin.perkLinks[groupName])
				{
					if (!perks.Contains(perk.Name))
					{
						Plugin.userPerkSettings[ev.Player.UserId].Add((IPerk)Activator.CreateInstance(perk));
					}
				}
			}
		}

		internal void OnPlayerHurt(HurtingEventArgs ev)
		{
			if (ev.Target.Health - ev.Amount <= 0)
			{
				Type t = typeof(CustomDeathReason);
				CustomDeathReason settings = (CustomDeathReason)Plugin.GetPerkSettings(ev.Target, t);
				if (Plugin.perkLinks.ContainsKey(ev.Target.GroupName) && Plugin.perkLinks[ev.Target.GroupName].Contains(t) && settings.Param != string.Empty)
				{
					ev.Target.Kill(settings.Param);
				}
			}
		}

		internal void OnIntercomUse(IntercomSpeakingEventArgs ev)
		{
			Type t = typeof(ExtendIntercom);
			ExtendIntercom settings = (ExtendIntercom)Plugin.GetPerkSettings(ev.Player, t);
			if (settings != null && Plugin.perkLinks.ContainsKey(ev.Player.GroupName) && Plugin.perkLinks[ev.Player.GroupName].Contains(t) && settings.Param == "on")
			{
				Intercom.host._speechTime = 30;
			}
		}
	}
}
