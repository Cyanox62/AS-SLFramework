using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Newtonsoft.Json;
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
	}
}
