using Exiled.API.Features;
using Exiled.Loader;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ExtraAdditions
{
	public class Plugin : Plugin<Config, Translation>
	{
		internal static Plugin singleton;

		private EventHandlers ev;

		internal static Dictionary<string, string> groups = new Dictionary<string, string>();

		public override void OnEnabled()
		{
			base.OnEnabled();

			singleton = this;

			ev = new EventHandlers();
			Exiled.Events.Handlers.Player.InteractingDoor += ev.OnDoorAccess;
			Exiled.Events.Handlers.Player.InteractingLocker += ev.OnLockerAccess;
			Exiled.Events.Handlers.Player.UnlockingGenerator += ev.OnGeneratorUnlock;
			Exiled.Events.Handlers.Player.InteractingElevator += ev.OnElevatorUse;

			Exiled.Events.Handlers.Server.RoundStarted += ev.OnRoundStart;
			Exiled.Events.Handlers.Server.RestartingRound += ev.OnRoundRestart;
		}

		public override void OnDisabled()
		{
			base.OnDisabled();

			Exiled.Events.Handlers.Player.InteractingDoor -= ev.OnDoorAccess;
			Exiled.Events.Handlers.Player.InteractingLocker -= ev.OnLockerAccess;
			Exiled.Events.Handlers.Player.UnlockingGenerator -= ev.OnGeneratorUnlock;
			Exiled.Events.Handlers.Player.InteractingElevator -= ev.OnElevatorUse;

			Exiled.Events.Handlers.Server.RoundStarted -= ev.OnRoundStart;
			Exiled.Events.Handlers.Server.RestartingRound -= ev.OnRoundRestart;

			ev = null;
		}

		internal static void AccessHintSystem(Player p, string hint, float time)
		{
			Loader.Plugins.FirstOrDefault(pl => pl.Name == "TipSystem")?.Assembly?.GetType("TipSystem.API.System")?.GetMethod("ShowHint", BindingFlags.Public | BindingFlags.Static)?.Invoke(null, new object[] { p, hint, time });
		}

		public override string Author => "Cyanox";
		public override string Name => "ExtraAdditions";
	}
}
