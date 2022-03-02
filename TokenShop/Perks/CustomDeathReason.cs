namespace TokenShop.Perks
{
	class CustomDeathReason : Perk
	{
		public override string PerkName => "Custom Death Reason";

		public string DeathReason = string.Empty;
	}
}