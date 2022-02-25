using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PatreonPerks.Perks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatreonPerks
{
	class EventHandlers
	{
		internal void OnRoundRestart()
		{
			File.WriteAllText(Plugin.GroupOverridesFile, JsonConvert.SerializeObject(Plugin.groups, Formatting.Indented));
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
				bool valid = true;
				for (int i = Plugin.userPerkSettings[ev.Player.UserId].Count - 1; i >= 0; i--)
				{
					var perk = Plugin.userPerkSettings[ev.Player.UserId][i];
					if (!Plugin.perkLinks.ContainsKey(ev.Player.GroupName) || (Plugin.perkLinks.ContainsKey(ev.Player.GroupName) && !Plugin.perkLinks[ev.Player.GroupName].Select(x => x.Name).Contains((string)((JObject)perk)["PerkName"])))
					{
						Log.Warn("removing");
						Plugin.userPerkSettings[ev.Player.UserId].RemoveAt(i);
						valid = false;
					}
				}
				if (!valid) File.WriteAllText(Plugin.UserSettings, JsonConvert.SerializeObject(Plugin.userPerkSettings, Formatting.Indented));
			}
		}

		internal void OnAssignGroup(ChangingGroupEventArgs ev)
		{
			string groupName = Plugin.GetGroupName(ev.NewGroup);
			if (Plugin.perkLinks.ContainsKey(groupName))
			{
				if (!Plugin.userPerkSettings.ContainsKey(ev.Player.UserId))
				{
					Plugin.userPerkSettings.Add(ev.Player.UserId, new List<object>());
				}
				foreach (Type perk in Plugin.perkLinks[groupName])
				{
					Plugin.userPerkSettings[ev.Player.UserId].Add((Perk)Activator.CreateInstance(perk));
				}
			}
		}

		internal void OnIntercomUse(IntercomSpeakingEventArgs ev)
		{
			Type t = typeof(ExtendIntercom);
			if (Plugin.perkLinks.ContainsKey(ev.Player.GroupName) && Plugin.perkLinks[ev.Player.GroupName].Contains(t))
			{
				Intercom.host.NetworkIntercomTime += 10;
			}
		}
	}
}
