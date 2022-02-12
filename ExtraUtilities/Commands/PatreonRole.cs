using CommandSystem;
using Exiled.API.Features;
using System;
using System.Linq;

namespace ExtraUtilities.Commands
{
	[CommandHandler(typeof(RemoteAdminCommandHandler))]
	class PatreonRole : ICommand
	{
		public string[] Aliases { get; set; } = Array.Empty<string>();

		public string Description { get; set; } = "Grants a patron a role";

		string ICommand.Command { get; } = "promote";

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			if (arguments.Count != 2)
			{
				Player target = Player.Get(arguments.ElementAt(0));
				if (target != null)
				{
					string tier = arguments.ElementAt(0);
					UserGroup userGroup = ServerStatic.PermissionsHandler.GetGroup(tier);
					if (userGroup != null)
					{
						target.Group = userGroup;
						response = $"Assigned user {target.Nickname} group {tier}";
						return true;
					}
					else
					{
						response = "Invalid group!";
						return false;
					}
				}
				else
				{
					response = "Invalid user!";
					return false;
				}
			}
			else
			{
				response = "Usage: PROMOTE [USER] [TIER]";
				return false;
			}
		}
	}
}
