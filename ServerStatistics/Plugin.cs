using Exiled.API.Features;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerStatistics
{
	public class Plugin : Plugin<Config>
	{
		internal static Plugin singleton;

		private Harmony hInstance;

		public override void OnEnabled()
		{
			base.OnEnabled();

			hInstance = new Harmony("cyan.serverstatistics");
			hInstance.PatchAll();

			singleton = this;
		}

		public override void OnDisabled()
		{
			base.OnDisabled();

			hInstance.UnpatchAll(hInstance.Id);
			hInstance = null;
		}

		public override string Author => "Cyanox";
		public override string Name => "ServerStatistics";
	}
}
