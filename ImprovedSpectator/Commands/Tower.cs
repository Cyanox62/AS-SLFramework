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
	class Tower : ICommand
	{
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
					if (player.Team == Team.RIP)
					{
						player.SetRole(RoleType.Tutorial);
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
