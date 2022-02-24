using Exiled.API.Features;
using Newtonsoft.Json;
using PatreonPerks.Commands;
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

		internal static Plugin singleton;

		private EventHandlers ev;

		internal static List<string> perkList = new List<string>();
		internal static Dictionary<string, List<string>> perkLinks = new Dictionary<string, List<string>>();
		internal static Dictionary<string, string> groups = new Dictionary<string, string>();

		public override void OnEnabled()
		{
			base.OnEnabled();

			singleton = this;

			ev = new EventHandlers();
			Exiled.Events.Handlers.Player.Verified += ev.OnPlayerVerified;

			Exiled.Events.Handlers.Server.RestartingRound += ev.OnRoundRestart;

			perkList = Assembly.GetExecutingAssembly().GetTypes().Where(type => type.Namespace == "PatreonPerks.Perks" && type.Name != "Perk").Select(x => x.Name).ToList();

			if (!Directory.Exists(FolderFilePath)) Directory.CreateDirectory(FolderFilePath);
			if (!File.Exists(PatreonPerkLinks)) File.WriteAllText(PatreonPerkLinks, "{}");
			if (!File.Exists(GroupOverridesFile)) File.WriteAllText(GroupOverridesFile, "{}");
			perkLinks = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(File.ReadAllText(PatreonPerkLinks));
			groups = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(GroupOverridesFile));
		}

		public override void OnDisabled()
		{
			base.OnDisabled();

			Exiled.Events.Handlers.Player.Verified -= ev.OnPlayerVerified;

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

		public override string Author => "Cyanox";
		public override string Name => "PatreonPerks";
	}
}
