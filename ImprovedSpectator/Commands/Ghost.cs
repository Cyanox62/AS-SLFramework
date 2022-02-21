using CommandSystem;
using Exiled.API.Features;
using MEC;
using RemoteAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ImprovedSpectator.Commands
{
	[CommandHandler(typeof(ClientCommandHandler))]
	class Ghost : ICommand
	{
		public string[] Aliases { get; set; } = Array.Empty<string>();

		public string Description { get; set; } = "Spawns you as a ghost";

		string ICommand.Command { get; } = "ghost";

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			if (sender is PlayerCommandSender p)
			{
				Player player = Player.Get(p);
				if (player != null)
				{
					if (player.Role.Team == Team.RIP || EventHandlers.additionalRespawnPlayers.Contains(player) || EventHandlers.ghostPlayers.Contains(player))
					{
						if (!EventHandlers.ghostPlayers.Contains(player)) EventHandlers.AddGhostPlayer(player);
						if (!EventHandlers.additionalRespawnPlayers.Contains(player)) EventHandlers.additionalRespawnPlayers.Add(player);
						player.SetRole(RoleType.Tutorial);
						List<Room> rooms = Room.Get(x => x.Type != Exiled.API.Enums.RoomType.Hcz939).ToList();
						Vector3 pos = rooms[UnityEngine.Random.Range(0, rooms.Count)].Position;
						pos.y += 2;
						Timing.CallDelayed(0.5f, () => player.Position = pos);
						response = "Respawned as a ghost.";
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
