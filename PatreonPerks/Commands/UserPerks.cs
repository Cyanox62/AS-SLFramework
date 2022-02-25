using CommandSystem;
using Exiled.API.Features;
using Newtonsoft.Json;
using PatreonPerks.Perks;
using RemoteAdmin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatreonPerks.Commands
{
	[CommandHandler(typeof(ClientCommandHandler))]
	class UserPerks : ICommand
	{
		public string[] Aliases { get; set; } = Array.Empty<string>();

		public string Description { get; set; } = "Uses patreon perks";

		string ICommand.Command { get; } = "perk";

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			if (sender is PlayerCommandSender p)
			{
				Player player = Player.Get(p);
				if (player != null)
				{
					string perk = arguments.ElementAt(0);
					if (perk.ToLower() == "list")
					{
						// list perks
						StringBuilder sb = new StringBuilder();
						sb.Append("Current perks:\n");
						for (int i = 0; i < Plugin.perkLinks[player.GroupName].Count; i++)
						{
							sb.Append($"- {Plugin.perkLinks[player.GroupName][i].Name}");
							if (i != Plugin.perkLinks[player.GroupName].Count - 1) sb.Append("\n");
						}
						response = sb.ToString();
						return true;
					}
					else if (arguments.Count == 2)
					{
						if (Plugin.perkLinks.ContainsKey(player.GroupName))
						{
							Type type = Plugin.perkLinks[player.GroupName].FirstOrDefault(x => x.Name.ToLower() == perk.ToLower());
							if (type != null)
							{
								if (type == typeof(AnnounceJoin))
								{
									string arg = arguments.ElementAt(1).ToLower();
									AnnounceJoin settings = (AnnounceJoin)Plugin.GetPerkSettings(player, type);
									if (settings != null)
									{
										if (arg == "on" || arg == "off")
										{
											settings.isEnabled = arg == "on" ? true : false;
										}
										response = $"{type.Name} has been toggled {(settings.isEnabled ? "on" : "off")}.";
									}
									else
									{
										response = "Failed to find user settings.";
										return false;
									}
									File.WriteAllText(Plugin.UserSettings, JsonConvert.SerializeObject(Plugin.userPerkSettings, Formatting.Indented));
									return true;
								}
								else if (type == typeof(CustomDeathReason))
								{
									string arg = arguments.ElementAt(1).ToLower();
									CustomDeathReason settings = (CustomDeathReason)Plugin.GetPerkSettings(player, type);
									if (settings != null)
									{
										if (arg != string.Empty)
										{
											settings.DeathReason = arg;
											response = $"{type.Name} has been set to '{settings.DeathReason}'.";
										}
										else
										{
											response = $"{type.Name} has been disabled.";
										}
									}
									else
									{
										response = "Failed to find user settings.";
										return false;
									}
									File.WriteAllText(Plugin.UserSettings, JsonConvert.SerializeObject(Plugin.userPerkSettings, Formatting.Indented));
									return true;
								}
								else if (type == typeof(ExtendIntercom))
								{
									string arg = arguments.ElementAt(1).ToLower();
									ExtendIntercom settings = (ExtendIntercom)Plugin.GetPerkSettings(player, type);
									if (settings != null)
									{
										if (arg == "on" || arg == "off")
										{
											settings.isEnabled = arg == "on" ? true : false;
										}
										response = $"{type.Name} has been toggled {(settings.isEnabled ? "on" : "off")}.";
									}
									else
									{
										response = "Failed to find user settings.";
										return false;
									}
									File.WriteAllText(Plugin.UserSettings, JsonConvert.SerializeObject(Plugin.userPerkSettings, Formatting.Indented));
									return true;
								}
								else
								{
									response = "Invalid perk!";
									return false;
								}
							}
							else
							{
								response = "Invalid perk!";
								return false;
							}
						}
						else
						{
							response = "You do not have access to this perk!";
							return false;
						}
					}
					response = "Usage: PERK [PERK NAME] [PARAMETER]";
					return false;
				}
				else
				{
					response = "Failed to grab player!";
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
