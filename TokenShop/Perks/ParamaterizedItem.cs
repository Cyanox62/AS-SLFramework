namespace TokenShop.Perks
{
	class ParamaterizedItem : Perk
	{
		private object param;
		private bool permanent;

		public ParamaterizedItem(object param, bool permanent)
		{
			this.param = param;
			this.permanent = permanent;
		}

		public bool IsPermanent => permanent;
		public object Param => param;

		public override string PerkName => "Round Item";
	}
}
