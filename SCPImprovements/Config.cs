using Exiled.API.Interfaces;
using System.ComponentModel;

namespace SCPImprovements
{
	public class Config : IConfig
	{
		public bool IsEnabled { get; set; } = true;

		[Description("How long in seconds to display the SCP-079 swap hint to the player.")]
		public float ComputerSwapHintTime { get; set; } = 10;

		[Description("How long in seconds to display the SCP-096 target hint player.")]
		public float Scp096TargetHintTime { get; set; } = 4;

		[Description("How long in seconds the SCP-096 relief hint should be shown.")]
		public float Scp096ReliefTime { get; set; } = 8;

		[Description("Formatting for where the hint is placed on screen. The higher the number, the lower the text goes.")]
		public int Scp079ReplaceTextLower { get; set; } = 5;

		[Description("Formatting for where the hint is placed on screen. The higher the number, the lower the text goes.")]
		public int Scp096VisionTextLower { get; set; } = 6;
	}
}
