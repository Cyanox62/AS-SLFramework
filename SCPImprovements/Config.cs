using Exiled.API.Interfaces;
using System.ComponentModel;

namespace SCPImprovements
{
	public class Config : IConfig
	{
		public bool IsEnabled { get; set; } = true;

		[Description("How long in seconds to display the SCP-079 swap hint to the player.")]
		public float ComputerSwapHintTime { get; set; } = 10;

		[Description("Formatting for where the hint is placed on screen. The higher the number, the lower the text goes.")]
		public int TextLower { get; set; } = 5;
	}
}
