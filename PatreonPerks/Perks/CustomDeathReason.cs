namespace PatreonPerks.Perks
{
	class CustomDeathReason : IPerk
	{
		public string PerkName { get; } = "CustomDeathReason";

		public string Param { get; set; } = string.Empty;
	}
}
