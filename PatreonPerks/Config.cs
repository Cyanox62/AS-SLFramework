using Exiled.API.Interfaces;
using System.Collections.Generic;

namespace PatreonPerks
{
	public class Config : IConfig
	{
		public bool IsEnabled { get; set; } = true;
	}
}
