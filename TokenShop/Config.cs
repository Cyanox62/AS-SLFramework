using Exiled.API.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;

namespace TokenShop
{
	public class Config : IConfig
	{
		public bool IsEnabled { get; set; } = true;

		[Description("Determines whether or not to print debug information.")]
		public bool IsDebug { get; set; } = false;

		[Description("Determines the shop items.")]
		public List<List<string>> ShopItems { get; set; } = new List<List<string>>()
		{
			new List<string>() { "1", "SCP-018", "P", "50" }
		};

		[Description("Determines the token multipliers for specified roles.")]
		public Dictionary<string, float> RoleTokenMultipliers { get; set; } = new Dictionary<string, float>();

		[Description("Determines how long in seconds a token grant hint should show for the player.")]
		public int TokenHintTime { get; set; } = 3;

		[Description("Determines how long in seconds before each playtime token drop.")]
		public int PlaytimeTokenInterval { get; set; } = 600;

		[Description("Determines how many tokens are given per playtime token drop.")]
		public int PlaytimeTokenAmount { get; set; } = 10;

		[Description("How many tokens a player gets for picking up a coin.")]
		public int CoinPickupTokens { get; set; } = 2;

		[Description("How many tokens a player gets for surviving the round as a human class.")]
		public int HumanSurviveTokens { get; set; } = 20;

		[Description("How many tokens a player gets for surviving the round as an SCP.")]
		public int ScpSurviveTokens { get; set; } = 10;

		[Description("Determines whether or not players in overwatch will be given tokens for playtime.")]
		public bool GiveOverwatchTokens{ get; set; } = false;

		[Description("Formatting for where the hint is placed on screen. The higher the number, the lower the text goes.")]
		public int TextLower { get; set; } = 4;
	}
}
