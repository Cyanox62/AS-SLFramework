using Exiled.API.Interfaces;
using System.ComponentModel;

namespace ExtraAdditions
{
	public class Config : IConfig
	{
		public bool IsEnabled { get; set; } = true;

		// Remote Keycard

		[Description("Determines if doors can be opened without holding a keycard.")]
		public bool RequireHeldKeycard { get; set; } = false;

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

		[Description("Formatting for where the hint is placed on screen. The higher the number, the lower the text goes.")]
		public int FlashlightHintTextLower { get; set; } = 8;
	}
}
