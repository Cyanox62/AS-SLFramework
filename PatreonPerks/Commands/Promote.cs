using CommandSystem;
using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PatreonPerks.Commands
{
	[CommandHandler(typeof(RemoteAdminCommandHandler))]
	class Promote : ICommand
	{
		public string[] Aliases { get; set; } = Array.Empty<string>();

		public string Description { get; set; } = "Grants a patron a role";

		string ICommand.Command { get; } = "promote";

		private GroupInfo IsValidGroup(string name)
		{
			Dictionary<string, UserGroup> groups = ServerStatic.PermissionsHandler.GetAllGroups();
			for (int i = 0; i < groups.Count; i++)
			{
				var entry = groups.ElementAt(i);
				string n = entry.Key.Trim().ToLower();
				if (n.StartsWith(name.Trim().ToLower()))
				{
					return new GroupInfo
					{
						group = entry.Value,
						groupName = n
					};
				}
			}
			return null;
		}

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			if (arguments.Count == 2)
			{
				Player target = Player.Get(arguments.ElementAt(0));
				if (target != null)
				{
					string tier = arguments.ElementAt(1);
					GroupInfo userGroup = IsValidGroup(tier);
					if (userGroup != null)
					{
						target.Group = userGroup.group;
						response = $"Assigned user {target.Nickname} group {userGroup.groupName}.";
						Plugin.groups.Add(target.UserId, userGroup.groupName);
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

	class GroupInfo
	{
		public string groupName;
		public UserGroup group;
	}
}
