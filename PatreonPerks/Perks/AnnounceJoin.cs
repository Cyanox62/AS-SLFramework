namespace PatreonPerks.Perks
{
	class AnnounceJoin : IPerk
	{
		public string PerkName { get; } = "AnnounceJoin";

		public string Param { get; set; } = "on";
	}
}