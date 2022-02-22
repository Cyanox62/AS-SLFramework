namespace TokenShop.Perks
{
	class RoundItem : Perk
	{
		private ItemType item;
		private bool permanent;

		public RoundItem(ItemType item, bool permanent)
		{
			this.item = item;
			this.permanent = permanent;
		}

		public bool IsPermanent
		{
			get
			{
				return permanent;
			}
		}

		public ItemType Item
		{
			get
			{
				return item;
			}
		}

		public override string PerkName => "Round Item";

		public override void GrantPerk() => Player.AddItem(item);
	}
}
