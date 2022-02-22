using CommandSystem;
using Exiled.API.Features;
using RemoteAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokenShop.Perks;

namespace TokenShop.Commands
{
	[CommandHandler(typeof(ClientCommandHandler))]
	class Shop : ICommand
	{
		public static string ShopString { get; set; }
		public static Dictionary<int, ShopItem> ShopItems { get; set; } = new Dictionary<int, ShopItem>();

		public string[] Aliases { get; set; } = Array.Empty<string>();

		public string Description { get; set; } = "Uses the shop";

		string ICommand.Command { get; } = "shop";

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			if (sender is PlayerCommandSender p)
			{
				Player player = Player.Get(p);
				if (arguments.Count == 0)
				{
					response = ShopString;
					return true;
				}
				else if (arguments.Count == 2)
				{
					string arg = arguments.ElementAt(0).ToLower();
					if (arg == "buy" || arg == "purchase")
					{
						if (int.TryParse(arguments.ElementAt(1), out int id))
						{
							// try purchase id
							if (EventHandlers.playerStats.ContainsKey(player.UserId))
							{
								// check tokens
								if (EventHandlers.playerStats[player.UserId].tokens >= ShopItems[id].price)
								{
									// purchase
									if (!EventHandlers.playerStats[player.UserId].perks.Contains(ShopItems[id]))
									{
										EventHandlers.playerStats[player.UserId].perks.Add(ShopItems[id]);
										EventHandlers.playerStats[player.UserId].tokens -= ShopItems[id].price;
										response = $"Successfully purchased shop item {id}!";
										return true;
									}
									else
									{
										response = "You already have that item!";
										return false;
									}
								}
								else
								{
									response = "You do not have enough coins for this item!";
									return false;
								}
							}
							else
							{
								response = "Error: Failed to find player stats";
								return true;
							}
						}
						else
						{
							response = "Invalid item id!";
							return false;
						}
					}
					else
					{
						response = "Usage: SHOP [BUY] [ITEM ID]";
						return false;
					}
				}
				else
				{
					response = "Usage: SHOP [BUY] [ITEM ID]";
					return false;
				}
			}
			else
			{
				response = "Only players may use this command!";
				return false;
			}
		}
	}
	
	public class ShopItem
	{
		public Perk perk;
		public int price;
	}
}
