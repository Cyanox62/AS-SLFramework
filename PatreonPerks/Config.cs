using Exiled.API.Interfaces;
using System.ComponentModel;

namespace PatreonPerks
{
	public class Config : IConfig
	{
		public bool IsEnabled { get; set; } = true;

		[Description("The amount of time someone with the ExtendIntercom perk has.")]
		public int ExtendIntercomTime { get; set; } = 30;

		[Description("The amount of time to display an AnnounceJoin perk.")]
		public ushort AnnounceJoinTime { get; set; } = 3;
	}
}
