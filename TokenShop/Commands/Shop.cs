using CommandSystem;
using Exiled.API.Features;
using RemoteAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TokenShop.Perks;

namespace TokenShop.Commands
{
	[CommandHandler(typeof(ClientCommandHandler))]
	class Shop : ICommand
	{
		public static StringBuilder ShopString { get; set; } = new StringBuilder();
		public static List<ShopItem> ShopItems { get; set; } = new List<ShopItem>();

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
					string r = ShopString.ToString();
					string[] rsp = r.Split('\n');

					if (EventHandlers.playerStats.ContainsKey(player.UserId))
					{
						bool isModified = false;
						for (int i = 0; i < rsp.Length; i++)
						{
							if (EventHandlers.playerStats[player.UserId].perks.ContainsKey(i))
							{
								rsp[i + 1] += " [PURCHASED]";
								isModified = true;
							}
						}

						if (isModified)
						{
							r = string.Empty;
							for (int i = 0; i < rsp.Length; i++)
							{
								r += rsp[i];
								if (i != rsp.Length - 1) r += "\n";
							}
						}
					}

					response = r;
					return true;
				}
				else if (arguments.Count == 2)
				{
					string arg = arguments.ElementAt(0).ToLower();
					if (arg == "buy" || arg == "purchase")
					{
						if (int.TryParse(arguments.ElementAt(1), out int id))
						{
							id -= 1;
							// try purchase id
							if (EventHandlers.playerStats.ContainsKey(player.UserId))
							{
								// check tokens
								ShopItem shopItem = ShopItems.FirstOrDefault(x => x.id == id);
								if (shopItem != null)
								{
									if (EventHandlers.playerStats[player.UserId].tokens >= shopItem.price)
									{
										// purchase
										if (!EventHandlers.playerStats[player.UserId].perks.ContainsKey(shopItem.id))
										{
											EventHandlers.playerStats[player.UserId].perks.Add(shopItem.id, shopItem.perk);
											EventHandlers.playerStats[player.UserId].tokens -= shopItem.price;
											response = $"Successfully purchased shop item {id + 1} for {shopItem.price} tokens!";
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
									response = "Invalid shop item!";
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
		public int id;
		public Perk perk;
		public int price;
	}
}
