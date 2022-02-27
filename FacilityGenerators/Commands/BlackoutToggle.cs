using CommandSystem;
using Exiled.API.Features;
using MEC;
using RemoteAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FacilityGenerators.Commands
{
	[CommandHandler(typeof(RemoteAdminCommandHandler))]
	class BlackoutToggle : ICommand
	{
		public string[] Aliases { get; set; } = { "bl" };

		public string Description { get; set; } = "Toggles blackouts";

		string ICommand.Command { get; } = "blackouts";

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			Room r = null;
			float f = float.MaxValue;
			foreach (Room room in Room.List)
			{
				if (room.Name.Contains("upstairs"))
				{
					Player.Get(sender as PlayerCommandSender).Position = room.transform.position;
				}
			}

			Log.Warn(r.Name);


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
