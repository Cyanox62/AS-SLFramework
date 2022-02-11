using Exiled.API.Interfaces;
using System.ComponentModel;

namespace PeanutExplodes
{
	public class Config : IConfig
	{
		public bool IsEnabled { get; set; } = true;

		[Description("The amount of grenades to go off upon explosion.")]
		public int Magnitude { get; set; } = 1;
	}
}
