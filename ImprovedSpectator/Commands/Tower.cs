using CommandSystem;
using Exiled.API.Features;
using Exiled.Loader;
using RemoteAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ImprovedSpectator.Commands
{
	[CommandHandler(typeof(ClientCommandHandler))]
	class Tower : ICommand
	{
		private Vector3 towerPos = new Vector3(54.7f, 1019.4f, -43.7f);

		public string[] Aliases { get; set; } = Array.Empty<string>();

		public string Description { get; set; } = "Respawns you in the tower";

		string ICommand.Command { get; } = "tower";

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			if (sender is PlayerCommandSender p)
			{
				Player player = Player.Get(p);

				var plugin = Loader.Plugins.First(pl => pl.Name == "TipSystem");
				var asm = plugin?.Assembly;
				var type = asm?.GetType("TipSystem.API.System");
				var m = type?.GetMethod("ShowHint", BindingFlags.Public | BindingFlags.Static);
				if (plugin != null && asm != null && type != null && m != null)
				{
					m.Invoke(null, new object[] { player, "\n\n\nthis is a test hint", 10f });
					m.Invoke(null, new object[] { player, "\n\n\n\n\n\n\nthis is a lower test hint", 20f });
				}

				if (player != null)
				{
					if (player.Role.Team == Team.RIP || EventHandlers.additionalRespawnPlayers.Contains(player) || EventHandlers.ghostPlayers.Contains(player))
					{
						if (!EventHandlers.ghostPlayers.Contains(player))
						{
							player.SetRole(RoleType.Tutorial);
						}
						else
						{
							player.Position = towerPos;
						}
						if (EventHandlers.ghostPlayers.Contains(player)) EventHandlers.RemoveGhostPlayer(player);
						if (!EventHandlers.additionalRespawnPlayers.Contains(player)) EventHandlers.additionalRespawnPlayers.Add(player);
						response = "Respawned as tutorial.";
					}
					else
					{
						response = "You can only use this command while dead.";
					}
				}
				else
				{
					response = "Player grab failed.";
				}
				return true;
			}
			else
			{
				response = "Only players may use this command";
				return false;
			}
		}
	}
}
