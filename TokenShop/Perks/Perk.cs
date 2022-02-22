using Exiled.API.Features;

namespace TokenShop.Perks
{
	public abstract class Perk
	{
		public Player Player { get; }

		public abstract string PerkName { get; }
	}
}
