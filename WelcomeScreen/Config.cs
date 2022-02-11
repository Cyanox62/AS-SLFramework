using Exiled.API.Interfaces;
using System.ComponentModel;

namespace WelcomeScreen
{
	public class Config : IConfig
	{
		public bool IsEnabled { get; set; } = true;

		[Description("The server number to be displayed.")]
		public int ServerNumber { get; set; } = 1;

		[Description("Formatting for where the hint is placed on screen. The higher the number, the lower the text goes.")]
		public int TextLower { get; set; } = 4;
	}
}
