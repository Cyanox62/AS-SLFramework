using CommandSystem;
using Exiled.API.Features;
using Newtonsoft.Json;
using PatreonPerks.Perks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatreonPerks.Commands
{
	[CommandHandler(typeof(RemoteAdminCommandHandler))]
	class LinkPerk : ICommand
	{
		public string[] Aliases { get; set; } = { "pl" };

		public string Description { get; set; } = "Links perks to groups";

		string ICommand.Command { get; } = "perklink";

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			if (arguments.Count == 0)
			{
				response = "Usage: PERKLINK [PERKS / LIST / ADD / REMOVE]";
				return true;
			}
			else if (arguments.Count == 1)
			{
				string arg = arguments.ElementAt(0).ToLower();
				if (arg == "perks")
				{
					if (Plugin.perkTypes.Count == 0)
					{
						response = "There are no perks loaded.";
						return true;
					}
					else
					{
						StringBuilder sb = new StringBuilder();
						sb.Append("Perk List:\n");
						for (int i = 0; i < Plugin.perkTypes.Count; i++)
						{
							sb.Append($"- {Plugin.perkTypes[i.ToString()]}");
							if (i != Plugin.perkTypes.Count - 1) sb.Append("\n");
						}
						response = sb.ToString();
						return true;
					}
				}
				else if (arg == "list")
				{
					if (Plugin.perkLinks.Count == 0)
					{
						response = "There are no perk links.";
						return true;
					}
					else
					{
						StringBuilder sb = new StringBuilder();
						sb.Append("Perk Links:\n");
						for (int i = 0; i < Plugin.perkLinks.Count; i++)
						{
							var entry = Plugin.perkLinks.ElementAt(i);
							sb.Append($"- {entry.Key} -> ");
							var t = Plugin.perkLinks.ElementAt(i).Value;
							for (int a = 0; a < t.Count; a++)
							{
								sb.Append(t[a].Name);
								if (a != t.Count - 1) sb.Append(", ");
							}
							if (i != Plugin.perkLinks.Count - 1) sb.Append("\n");
						}
						response = sb.ToString();
						return true;
					}
				}
				else if (arg == "add" || arg == "remove")
				{
					response = "Usage: PERKLINK [ADD / REMOVE] [GROUP] [PERKNAME]";
					return false;
				}
				else
				{
					response = "Usage: PERKLINK [PERKS / LIST / ADD / REMOVE]";
					return false;
				}
			}
			else if (arguments.Count == 3)
			{
				string tier = arguments.ElementAt(1);
				GroupInfo userGroup = Plugin.IsValidGroup(tier);
				if (userGroup != null)
				{
					string name = arguments.ElementAt(2);
					if (Plugin.perkTypes.ContainsKey(name))
					{
						Type t = Plugin.perkTypes[name];
						string arg = arguments.ElementAt(0).ToLower();
						if (arg == "add")
						{
							if (!Plugin.perkLinks.ContainsKey(userGroup.groupName))
							{
								Plugin.perkLinks.Add(userGroup.groupName, new List<Type>());
							}

							if (!Plugin.perkLinks[userGroup.groupName].Contains(t))
							{
								Plugin.perkLinks[userGroup.groupName].Add(t);
								response = $"Linked perk '{name}' to group {userGroup.groupName}.";
							}
							else
							{
								response = $"Group {userGroup.groupName} already has {name} linked!";
								return false;
							}

							/*foreach (var entry in Plugin.userPerkSettings)
							{
								if (entry.Key.GroupName == userGroup.groupName)
								{
									entry.Value.Add((Perk)Activator.CreateInstance(t));
								}
							}*/
						}
						else if (arg == "remove")
						{
							if (Plugin.perkLinks.ContainsKey(userGroup.groupName))
							{
								Plugin.perkLinks[userGroup.groupName].Remove(t);
								if (Plugin.perkLinks[userGroup.groupName].Count == 0)
								{
									Plugin.perkLinks.Remove(userGroup.groupName);
								}
							}
							response = $"Removed perk link '{name}' from group {userGroup.groupName}.";

							/*foreach (var entry in Plugin.userPerkSettings)
							{
								if (entry.Key.GroupName == userGroup.groupName && entry.Value.Select().Contains(t))
								{
									entry.Value.Add((Perk)Activator.CreateInstance(t));
								}
							}*/
						}
						else
						{
							response = "Usage: PERKLINK [ADD / REMOVE] [GROUP] [PERKNAME]";
							return false;
						}
						File.WriteAllText(Plugin.PatreonPerkLinks, JsonConvert.SerializeObject(Plugin.perkLinks, Formatting.Indented));
						return true;
					}
					else
					{
						response = "Invalid perk name!";
						return false;
					}
				}
				else
				{
					response = "Invalid group!";
					return false;
				}
			}
			else
			{
				response = "Usage: PERKLINK [PERKS / LIST / ADD / REMOVE]";
				return true;
			}
		}
	}
}
