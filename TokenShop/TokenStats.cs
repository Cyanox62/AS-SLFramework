using System.Collections.Generic;
using TokenShop.Commands;
using TokenShop.Perks;

namespace TokenShop
{
	public class TokenStats
	{
		public int playtime = 0;
		public int tokens = 0;
		public Dictionary<int, object> perks = new Dictionary<int, object>();

		internal string path;
	}
}
