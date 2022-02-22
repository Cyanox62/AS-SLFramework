using CommandSystem;
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
						// purchase id
						response = "not yet lol";
						return true;
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
	}
	
	public class ShopItem
	{
		public Perk perk;
		public int price;
	}
}
