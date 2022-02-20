using Exiled.API.Interfaces;
using System.ComponentModel;

namespace FacilityGenerators
{
	public class Config : IConfig
	{
		public bool IsEnabled { get; set; } = true;

		[Description("Determines if debug information will be printed to the console.")]
		public bool DebugMode { get; set; } = false;

		[Description("The amount of flashlights to spawn randomly around the map.")]
		public int FlashlightsToSpawn { get; set; } = 16;

		[Description("Determines the minimum amount of blackouts that can happen in a round.")]
		public int MinBlackoutsPerRound { get; set; } = 0;

		[Description("Determines the maximum amount of blackouts that can happen in a round.")]
		public int MaxBlackoutsPerRound { get; set; } = 2;

		[Description("Determines the minimum amount of time in seconds between each blackout.")]
		public int MinTimeBetweenBlackouts { get; set; } = 300;

		[Description("Determines the maximum amount of time in seconds between each blackout.")]
		public int MaxTimeBetweenBlackouts { get; set; } = 900;

		[Description("Determines the minimum amount of time in seconds a blackout can last for.")]
		public int MinBlackoutDuration { get; set; } = 25;

		[Description("Determines the maximum amount of time in seconds a blackout can last for.")]
		public int MaxBlackoutDuration { get; set; } = 52;

		[Description("The message for CASSIE to say when a blackout starts.")]
		public string CassieBlackoutStart { get; set; } = "error . facility power system failure detected";

		[Description("The offset betweeen when CASSIE begins his announcement and when the lights go off.")]
		public float CassieBlackoutStartOffset { get; set; } = 5f;

		[Description("The message for CASSIE to say when a blackout ends.")]
		public string CassieBlackoutEnd { get; set; } = "facility power system now operational";

		[Description("The offset betweeen when CASSIE begins his announcement and when the lights turn on.")]
		public float CassieBlackoutEndOffset { get; set; } = 5f;
	}
}
