using Exiled.API.Interfaces;
using System.ComponentModel;

namespace FacilityGenerators
{
	public class Config : IConfig
	{
		public bool IsEnabled { get; set; } = true;

		[Description("The amount of flashlights to spawn randomly around the map.")]
		public int FlashlightsToSpawn { get; set; } = 16;

		[Description("Determines the minimum amount of blackouts that can happen in a round.")]
		public int MinBlackoutsPerRound { get; set; } = 0;

		[Description("Determines the maximum amount of blackouts that can happen in a round.")]
		public int MaxBlackoutsPerRound { get; set; } = 2;
	}
}
