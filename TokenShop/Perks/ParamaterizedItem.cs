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

		public bool IsPermanent
		{
			get
			{
				return permanent;
			}
		}

		public object Param
		{
			get
			{
				return param;
			}
		}

		public override string PerkName => "Round Item";
	}
}
