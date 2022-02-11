using CommandSystem;
using Exiled.API.Features;
using RemoteAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImprovedSpectator.Commands
{
	[CommandHandler(typeof(ClientCommandHandler))]
	class Ghost : ICommand
	{
		public string[] Aliases { get; set; } = Array.Empty<string>();

		public string Description { get; set; } = "Report something";

		string ICommand.Command { get; } = "report";

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			if (sender is PlayerCommandSender p)
			{
				Player player = Player.Get(p);
				if (player != null)
				{
					if (player.Team == Team.RIP)
					{
						if (!EventHandlers.ghostPlayers.Contains(player)) EventHandlers.AddGhostPlayer(player);
						if (!EventHandlers.additionalRespawnPlayers.Contains(player)) EventHandlers.additionalRespawnPlayers.Add(player);
						player.SetRole(RoleType.Tutorial);
						player.Position = Map.Rooms[UnityEngine.Random.Range(0, Map.Rooms.Count)].Position;
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
