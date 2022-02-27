using Exiled.API.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;

namespace ExtraAdditions
{
	public class Config : IConfig
	{
		public bool IsEnabled { get; set; } = true;

		// Remote Keycard

		[Description("Determines if doors can be opened without holding a keycard.")]
		public bool RequireHeldKeycard { get; set; } = false;

		[Description("Determines the time in seconds to display the access denied keycard hint.")]
		public float AccessDeniedHintTime { get; set; } = 3f;

		[Description("Formatting for where the hint is placed on screen. The higher the number, the lower the text goes.")]
		public int KeycardHintTextLower { get; set; } = 6;

		// Elevator Failure

		[Description("Determines the minimum time in seconds an elevator can fail for.")]
		public int MinElevatorFailTime { get; set; } = 30;

		[Description("Determines the maximum time in seconds an elevator can fail for.")]
		public int MaxElevatorFailTime { get; set; } = 60;

		[Description("Determines the minimum time in seconds between two elevators failing.")]
		public int MinTimeBetweenElevatorFails { get; set; } = 10;

		[Description("Determines the maximum time in seconds between two elevators failing.")]
		public int MaxTimeBetweenElevatorFails { get; set; } = 30;

		[Description("Determines how long in seconds to show the broken elevator hint for.")]
		public int BrokenElevatorHintTime { get; set; } = 5;

		[Description("Formatting for where the hint is placed on screen. The higher the number, the lower the text goes.")]
		public int ElevatorHintTextLower { get; set; } = 7;

		// Flashlight Battery

		[Description("How long in seconds a flashlight battery lasts for.")]
		public int FlashlightBattery { get; set; } = 240;

		[Description("Formatting for where the hint is placed on screen. The higher the number, the lower the text goes.")]
		public int FlashlightHintTextLower { get; set; } = 10;

		// Autonuke

		[Description("Determines if the nuke can be stopped after automatically starting.")]
		public bool CanStopAutonuke { get; set; } = false;

		[Description("The amount of time in seconds before the nuke starts.")]
		public float TimeUntilAutonuke { get; set; } = 900;

		[Description("At what time intervals after autonuke starts to announce certain CASSIE announcements.")]
		public Dictionary<float, string> CassieNukeAnnouncements { get; set; } = new Dictionary<float, string>();

		// Item Spawning

		[Description("The percent chance of each random bench spawn.")]
		public float BenchSpawnChance { get; set; } = 75;

		[Description("The items that can spawn on benches along with their weights. Weights should add up to 100.")]
		public Dictionary<ItemType, float> BenchItemSpawnWeights { get; set; } = new Dictionary<ItemType, float>()
		{
			{ ItemType.Medkit, 25f },
			{ ItemType.Adrenaline, 25f },
			{ ItemType.Coin, 25f },
			{ ItemType.Ammo12gauge, 5f },
			{ ItemType.Ammo44cal, 5f },
			{ ItemType.Ammo556x45, 5f },
			{ ItemType.Ammo762x39, 5f },
			{ ItemType.Ammo9x19, 5f },
		};

		[Description("The items to spawn randomly on entrance zone desks.")]
		public List<ItemType> DeskItems { get; set; } = new List<ItemType>()
		{
			ItemType.KeycardZoneManager,
			ItemType.KeycardZoneManager
		};
	}
}
