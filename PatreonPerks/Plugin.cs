using Exiled.API.Features;
using Newtonsoft.Json;
using PatreonPerks.Commands;
using PatreonPerks.Perks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace PatreonPerks
{
    public class Plugin : Plugin<Config>
    {
        internal static string FolderFilePath = Path.Combine(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EXILED"), "Plugins"), "PatreonPerks");
        internal static string PatreonPerkLinks = Path.Combine(Path.Combine(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EXILED"), "Plugins"), "PatreonPerks"), "perkLinks.json");
        internal static string GroupOverridesFile = Path.Combine(Path.Combine(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EXILED"), "Plugins"), "PatreonPerks"), "playerRankOverrides.json");
        internal static string UserSettings = Path.Combine(Path.Combine(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EXILED"), "Plugins"), "PatreonPerks"), "userPerkSettings.json");

		internal static Plugin singleton;

		private EventHandlers ev;

		// Lists all perks
		// perkName, perkType
		internal static Dictionary<string, Type> perkTypes = new Dictionary<string, Type>();

		// Tracks group to perk lists
		// groupName, perkTypes
		internal static Dictionary<string, List<Type>> perkLinks = new Dictionary<string, List<Type>>();

		// Tracks player instances of perks
		internal static Dictionary<string, List<object>> userPerkSettings = new Dictionary<string, List<object>>();

		// Saving player promotions
		internal static Dictionary<string, string> groups = new Dictionary<string, string>();

		public override void OnEnabled()
		{
			base.OnEnabled();

			singleton = this;

			ev = new EventHandlers();
			Exiled.Events.Handlers.Player.Verified += ev.OnPlayerVerified;
			Exiled.Events.Handlers.Player.IntercomSpeaking += ev.OnIntercomUse;
			Exiled.Events.Handlers.Player.ChangingGroup += ev.OnAssignGroup;

			Exiled.Events.Handlers.Server.RestartingRound += ev.OnRoundRestart;

			foreach (var a in Assembly.GetExecutingAssembly().GetTypes().Where(type => type.Namespace == "PatreonPerks.Perks" && type.Name != "Perk"))
			{
				perkTypes.Add(a.Name, a);
			}

			if (!Directory.Exists(FolderFilePath)) Directory.CreateDirectory(FolderFilePath);
			if (!File.Exists(PatreonPerkLinks)) File.WriteAllText(PatreonPerkLinks, "{}");
			if (!File.Exists(GroupOverridesFile)) File.WriteAllText(GroupOverridesFile, "{}");
			if (!File.Exists(UserSettings)) File.WriteAllText(UserSettings, "{}");
			perkLinks = JsonConvert.DeserializeObject<Dictionary<string, List<Type>>>(File.ReadAllText(PatreonPerkLinks));
			userPerkSettings = JsonConvert.DeserializeObject<Dictionary<string, List<object>>>(File.ReadAllText(UserSettings));
			groups = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(GroupOverridesFile));
		}

		public override void OnDisabled()
		{
			base.OnDisabled();

			Exiled.Events.Handlers.Player.Verified -= ev.OnPlayerVerified;
			Exiled.Events.Handlers.Player.IntercomSpeaking -= ev.OnIntercomUse;
			Exiled.Events.Handlers.Player.ChangingGroup -= ev.OnAssignGroup;

			Exiled.Events.Handlers.Server.RestartingRound -= ev.OnRoundRestart;

			ev = null;
		}

		internal static GroupInfo IsValidGroup(string name)
		{
			Dictionary<string, UserGroup> groups = ServerStatic.PermissionsHandler.GetAllGroups();
			for (int i = 0; i < groups.Count; i++)
			{
				var entry = groups.ElementAt(i);
				string n = entry.Key.Trim().ToLower();
				if (n.StartsWith(name.Trim().ToLower()))
				{
					return new GroupInfo
					{
						group = entry.Value,
						groupName = n
					};
				}
			}
			return null;
		}

		internal static string GetGroupName(UserGroup group)
		{
			Dictionary<string, UserGroup> groups = ServerStatic.PermissionsHandler.GetAllGroups();
			foreach (var g in groups)
			{
				if (g.Value == group) return g.Key;
			}
			return null;
		}

		internal static object GetPerkSettings(Player p, Type t)
		{
			foreach (Perk perk in Plugin.userPerkSettings[p.UserId])
			{
				if (t.IsAssignableFrom(perk.GetType())) return perk;
			}
			return null;
		}

		public override string Author => "Cyanox";
		public override string Name => "PatreonPerks";
	}
}
