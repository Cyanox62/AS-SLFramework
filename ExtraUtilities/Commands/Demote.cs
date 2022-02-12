using CommandSystem;
using Exiled.API.Features;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;

namespace ExtraUtilities.Commands
{
	[CommandHandler(typeof(RemoteAdminCommandHandler))]
	class Demopte : ICommand
	{
		public string[] Aliases { get; set; } = Array.Empty<string>();

		public string Description { get; set; } = "Removes patron roles";

		string ICommand.Command { get; } = "demote";

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			if (arguments.Count == 1)
			{
				Player target = Player.Get(arguments.ElementAt(0));
				if (target != null)
				{
					if (Plugin.groups.ContainsKey(target.UserId))
					{
						Plugin.groups.Remove(target.UserId);
						target.Group = null;
						File.WriteAllText(Plugin.GroupOverridesFile, JsonConvert.SerializeObject(Plugin.groups, Formatting.Indented));
						response = $"Removed role overrides for user {target.Nickname}.";
						return true;
					}
					else
					{
						response = "User doesn't have a role override.";
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
				response = "Usage: DEMOTE [USER]";
				return false;
			}
		}
	}
}
