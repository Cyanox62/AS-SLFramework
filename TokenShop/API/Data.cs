namespace TokenShop.API
{
	public static class Data
	{
		public static int GetTokens(string userid) => EventHandlers.playerStats.ContainsKey(userid) ? EventHandlers.playerStats[userid].tokens : -1;
		public static int GetPlaytime(string userid) => EventHandlers.playerStats.ContainsKey(userid) ? EventHandlers.playerStats[userid].playtime : -1;
	}
}
