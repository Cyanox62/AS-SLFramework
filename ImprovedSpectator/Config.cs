using Exiled.API.Interfaces;
using System.ComponentModel;

namespace ImprovedSpectator
{
	public class Config : IConfig
	{
		public bool IsEnabled { get; set; } = true;

		[Description("Formatting for where the hint is placed on screen. The higher the number, the lower the text goes.")]
		public int TextLower { get; set; } = 4;
	}
}
