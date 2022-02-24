using Exiled.API.Features;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatreonPerks
{
    public class Plugin : Plugin<Config>
    {
        internal static string FolderFilePath = Path.Combine(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EXILED"), "Plugins"), "PatreonPerks");
        internal static string GroupOverridesFile = Path.Combine(Path.Combine(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EXILED"), "Plugins"), "PatreonPerks"), "playerRankOverrides.json");

		internal static Plugin singleton;

		private EventHandlers ev;

		internal static Dictionary<string, string> groups = new Dictionary<string, string>();

		public override void OnEnabled()
		{
			base.OnEnabled();

			singleton = this;

			ev = new EventHandlers();
			Exiled.Events.Handlers.Player.Verified += ev.OnPlayerVerified;

			Exiled.Events.Handlers.Server.RestartingRound += ev.OnRoundRestart;

			if (!Directory.Exists(FolderFilePath)) Directory.CreateDirectory(FolderFilePath);
			if (!File.Exists(GroupOverridesFile)) File.WriteAllText(GroupOverridesFile, "{}");
			groups = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(GroupOverridesFile));
		}

		public override void OnDisabled()
		{
			base.OnDisabled();

			Exiled.Events.Handlers.Player.Verified -= ev.OnPlayerVerified;

			Exiled.Events.Handlers.Server.RestartingRound -= ev.OnRoundRestart;

			ev = null;
		}

		public override string Author => "Cyanox";
		public override string Name => "PatreonPerks";
	}
}
