namespace PatreonPerks.Perks
{
	public interface IPerk
	{
		string PerkName { get; }
		string Param { get; set; }
	}
}