using CommandSystem;
using Exiled.API.Features;
using RemoteAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
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
				if (player != null)
				{
					if (player.Team == Team.RIP || EventHandlers.additionalRespawnPlayers.Contains(player) || EventHandlers.ghostPlayers.Contains(player))
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
