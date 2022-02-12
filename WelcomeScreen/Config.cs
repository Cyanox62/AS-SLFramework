using Exiled.API.Interfaces;
using System.ComponentModel;

namespace WelcomeScreen
{
	public class Config : IConfig
	{
		public bool IsEnabled { get; set; } = true;

		[Description("How long to display the welcome broadcast to a player upon joining. Set to 0 to disable.")]
		public int BroadcastTime { get; set; } = 5;

		[Description("Determines if the hint should be displayed on the waiting for players screen.")]
		public bool ShowHint { get; set; } = true;

		[Description("The server number to be displayed in the hint.")]
		public int ServerNumber { get; set; } = 1;

		[Description("Formatting for where the hint is placed on screen. The higher the number, the lower the text goes.")]
		public int TextLower { get; set; } = 4;
	}
}
