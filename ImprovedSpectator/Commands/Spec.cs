using CommandSystem;
using Exiled.API.Features;
using RemoteAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Exiled.API.Extensions;

namespace ImprovedSpectator.Commands
{
	[CommandHandler(typeof(ClientCommandHandler))]
	class Spec : ICommand
	{
		public string[] Aliases { get; set; } = Array.Empty<string>();

		public string Description { get; set; } = "Respawns you as a spectator.";

		string ICommand.Command { get; } = "spec";

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			if (sender is PlayerCommandSender p)
			{
				Player player = Player.Get(p);
				if (player != null)
				{
					if (player.Team == Team.RIP || EventHandlers.additionalRespawnPlayers.Contains(player) || EventHandlers.ghostPlayers.Contains(player))
					{
						if (EventHandlers.ghostPlayers.Contains(player)) EventHandlers.RemoveGhostPlayer(player);
						if (EventHandlers.additionalRespawnPlayers.Contains(player)) EventHandlers.additionalRespawnPlayers.Remove(player);
						player.SetRole(RoleType.Spectator);
						response = "Moved back to spectator.";
					}
					else
					{
						response = "You may only use this command while dead.";
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
