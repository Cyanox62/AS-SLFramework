using Exiled.API.Interfaces;
using System.ComponentModel;

namespace WelcomeScreen
{
	public class Config : IConfig
	{
		public bool IsEnabled { get; set; } = true;

		[Description("The server number to be displayed.")]
		public int ServerNumber { get; set; } = 1;

		[Description("The hint to display, use {serverNum} to represent the server number.")]
		public string Hint { get; set; } = "\n\n\n\nServer {serverNum}\ndiscord.gg/yourlink";
	}
}
