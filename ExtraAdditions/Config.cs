using Exiled.API.Interfaces;
using System.ComponentModel;

namespace ExtraUtilities
{
	public class Config : IConfig
	{
		public bool IsEnabled { get; set; } = true;

		[Description("Determines if doors can be opened without holding a keycard.")]
		public bool RequireHeldKeycard { get; set; } = false;
	}
}
