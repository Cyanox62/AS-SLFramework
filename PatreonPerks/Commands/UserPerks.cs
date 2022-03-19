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
					if (arguments.Count == 0)
					{
						response = "Usage: PERK [PERK NAME] [PARAMETER]";
						return false;
					}
					string perk = arguments.ElementAt(0);
					if (perk.ToLower() == "list")
					{
						StringBuilder sb = new StringBuilder();
						sb.Append("Current perks:\n");
						for (int i = 0; i < Plugin.perkLinks[player.GroupName].Count; i++)
						{
							Type t = Plugin.perkLinks[player.GroupName][i];
							object settings = Plugin.GetPerkSettings(player, t);
							IPerk cast = (IPerk)settings;
							sb.Append($"- {t.Name} | {(cast.Param == string.Empty ? "None" : cast.Param)}");
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
								object settings = Plugin.GetPerkSettings(player, type);
								if (type == typeof(AnnounceJoin))
								{
									string arg = arguments.ElementAt(1).ToLower();
									AnnounceJoin setSettings = (AnnounceJoin)settings;
									if (settings != null)
									{
										if (arg == "on" || arg == "off")
										{
											setSettings.Param = arg.ToLower();
										}
										response = $"{type.Name} has been toggled {setSettings.Param}.";
									}
									else
									{
										response = "Failed to find user settings.";
										return false;
									}
									File.WriteAllText(Plugin.UserSettings, JsonConvert.SerializeObject(Plugin.userPerkSettings, Formatting.Indented, Plugin.userSerializeSettings));
									return true;
								}
								else if (type == typeof(CustomDeathReason))
								{
									string arg = arguments.ElementAt(1).ToLower();
									CustomDeathReason setSettings = (CustomDeathReason)settings;
									if (settings != null)
									{
										if (arg != string.Empty)
										{
											setSettings.Param = arg;
											response = $"{type.Name} has been set to '{setSettings.Param}'.";
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
									File.WriteAllText(Plugin.UserSettings, JsonConvert.SerializeObject(Plugin.userPerkSettings, Formatting.Indented, Plugin.userSerializeSettings));
									return true;
								}
								else if (type == typeof(ExtendIntercom))
								{
									string arg = arguments.ElementAt(1).ToLower();
									ExtendIntercom setSettings = (ExtendIntercom)settings;
									if (settings != null)
									{
										if (arg == "on" || arg == "off")
										{
											setSettings.Param = arg.ToLower();
											response = $"{type.Name} has been toggled {setSettings.Param}.";
											File.WriteAllText(Plugin.UserSettings, JsonConvert.SerializeObject(Plugin.userPerkSettings, Formatting.Indented, Plugin.userSerializeSettings));
											return true;
										}
										else
										{
											response = "Unknown parameter.";
											return false;
										}
									}
									else
									{
										response = "Failed to find user settings.";
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
					else
					{
						response = "Usage: PERK [LIST / PERK NAME] (PARAMETER)";
						return false;
					}
				}
				else
				{
					response = "Failed to grab player!";
					return false;
				}
			}
			else
			{
				response = "Usage: PERK [PERK NAME] [PARAMETER]";
				return false;
			}
		}
	}
}
