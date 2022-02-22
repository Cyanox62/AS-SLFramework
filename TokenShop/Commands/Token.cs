using CommandSystem;
using Exiled.API.Features;
using System;
using System.Linq;

namespace TokenShop.Commands
{
	[CommandHandler(typeof(RemoteAdminCommandHandler))]
	class Token : ICommand
	{
		public string[] Aliases { get; set; } = Array.Empty<string>();

		public string Description { get; set; } = "Alters a player's tokens";

		string ICommand.Command { get; } = "token";

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			if (arguments.Count == 3)
			{
				Player target = Player.Get(arguments.ElementAt(1));
				if (target != null)
				{
					if (int.TryParse(arguments.ElementAt(2), out int tokens) && tokens > 0)
					{
						string cmd = arguments.ElementAt(0).ToLower();
						if (cmd == "give")
						{
							EventHandlers.GiveTokens(target, tokens, "<color=red>[ADMIN COMMAND]</color>");
							response = $"Gave {target.Nickname} {tokens} tokens.";
							return true;
						}
						else if (cmd == "remove")
						{
							EventHandlers.GiveTokens(target, -tokens, "<color=red>[ADMIN COMMAND]</color>", false);
							response = $"Removed {tokens} tokens from {target.Nickname}.";
							return true;
						}
						else
						{
							response = "Invalid operations!";
							return false;
						}
					}
					else
					{
						response = "Invalid token amount!";
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
				response = "Usage: TOKEN [GIVE / REMOVE] [USER] [AMOUNT]";
				return false;
			}
		}
	}
}
