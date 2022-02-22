using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokenShop.Commands;

namespace TokenShop
{
	public class TokenStats
	{
		public int playtime = 0;
		public int tokens = 0;
		public List<ShopItem> perks = new List<ShopItem>();

		internal string path;
	}
}
