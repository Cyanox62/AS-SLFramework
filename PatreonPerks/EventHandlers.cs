using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Newtonsoft.Json;
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
		}

		internal void OnAssignGroup(ChangingGroupEventArgs ev)
		{
			string groupName = Plugin.GetGroupName(ev.NewGroup);
			if (Plugin.perkLinks.ContainsKey(groupName))
			{
				if (!Plugin.userPerkSettings.ContainsKey(ev.Player))
				{
					Plugin.userPerkSettings.Add(ev.Player, new List<Perk>());
				}
				foreach (Type perk in Plugin.perkLinks[groupName])
				{
					Plugin.userPerkSettings[ev.Player].Add((Perk)Activator.CreateInstance(perk));
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
