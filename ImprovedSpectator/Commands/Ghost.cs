﻿using CommandSystem;
using Exiled.API.Features;
using MEC;
using RemoteAdmin;
using System;
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
					if (player.Team == Team.RIP || EventHandlers.additionalRespawnPlayers.Contains(player))
					{
						if (!EventHandlers.ghostPlayers.Contains(player)) EventHandlers.AddGhostPlayer(player);
						if (!EventHandlers.additionalRespawnPlayers.Contains(player)) EventHandlers.additionalRespawnPlayers.Add(player);
						player.SetRole(RoleType.Tutorial);
						Vector3 pos = Map.Rooms[UnityEngine.Random.Range(0, Map.Rooms.Count)].Position;
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
