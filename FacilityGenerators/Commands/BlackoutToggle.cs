using CommandSystem;
using MEC;
using RemoteAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacilityGenerators.Commands
{
	[CommandHandler(typeof(RemoteAdminCommandHandler))]
	class BlackoutToggle : ICommand
	{
		public string[] Aliases { get; set; } = Array.Empty<string>();

		public string Description { get; set; } = "Toggles blackouts";

		string ICommand.Command { get; } = "blackout";

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			EventHandlers.isBlackoutEnabled = !EventHandlers.isBlackoutEnabled;
			if (!EventHandlers.isBlackoutEnabled && EventHandlers.coroutine.IsRunning)
			{
				Timing.KillCoroutines(EventHandlers.coroutine);
			}
			response = $"Blackouts have been toggled {(EventHandlers.isBlackoutEnabled ? "on" : "off")}";
			return true;
		}
	}
}
