using Exiled.API.Features;
using Exiled.Loader;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ExtraAdditions
{
	public class Plugin : Plugin<Config, Translation>
	{
		internal static Plugin singleton;

		private ElevatorFailure.EventHandlers elevatorFailureEvents;
		private RemoteKeycard.EventHandlers remoteKeycardEvents;
		private FlashlightBattery.EventHandlers flashlightBatteryEvents;

		internal static Dictionary<string, string> groups = new Dictionary<string, string>();

		public override void OnEnabled()
		{
			base.OnEnabled();

			singleton = this;

			// Remote Keycard

			remoteKeycardEvents = new RemoteKeycard.EventHandlers();

			Exiled.Events.Handlers.Player.InteractingDoor += remoteKeycardEvents.OnDoorAccess;
			Exiled.Events.Handlers.Player.InteractingLocker += remoteKeycardEvents.OnLockerAccess;
			Exiled.Events.Handlers.Player.UnlockingGenerator += remoteKeycardEvents.OnGeneratorUnlock;

			// Elevator Failure

			elevatorFailureEvents = new ElevatorFailure.EventHandlers();

			Exiled.Events.Handlers.Server.RoundStarted += elevatorFailureEvents.OnRoundStart;
			Exiled.Events.Handlers.Server.RestartingRound += elevatorFailureEvents.OnRoundRestart;

			Exiled.Events.Handlers.Player.InteractingElevator += elevatorFailureEvents.OnElevatorUse;

			// Flashlight Battery

			flashlightBatteryEvents = new FlashlightBattery.EventHandlers();

			Exiled.Events.Handlers.Server.RestartingRound += flashlightBatteryEvents.OnRoundRestart;

			Exiled.Events.Handlers.Player.DroppingItem += flashlightBatteryEvents.OnDroppingItem;
			Exiled.Events.Handlers.Player.PickingUpItem += flashlightBatteryEvents.OnPickingUpItem;
			Exiled.Events.Handlers.Player.Spawning += flashlightBatteryEvents.OnSpawn;
		}

		public override void OnDisabled()
		{
			base.OnDisabled();

			// Remote Keycard

			Exiled.Events.Handlers.Player.InteractingDoor -= remoteKeycardEvents.OnDoorAccess;
			Exiled.Events.Handlers.Player.InteractingLocker -= remoteKeycardEvents.OnLockerAccess;
			Exiled.Events.Handlers.Player.UnlockingGenerator -= remoteKeycardEvents.OnGeneratorUnlock;

			remoteKeycardEvents = null;

			// Elevator Failure

			Exiled.Events.Handlers.Server.RoundStarted -= elevatorFailureEvents.OnRoundStart;
			Exiled.Events.Handlers.Server.RestartingRound -= elevatorFailureEvents.OnRoundRestart;

			Exiled.Events.Handlers.Player.InteractingElevator -= elevatorFailureEvents.OnElevatorUse;

			elevatorFailureEvents = null;

			// Flashlight Battery

			Exiled.Events.Handlers.Server.RestartingRound -= flashlightBatteryEvents.OnRoundRestart;

			Exiled.Events.Handlers.Player.DroppingItem -= flashlightBatteryEvents.OnDroppingItem;
			Exiled.Events.Handlers.Player.PickingUpItem -= flashlightBatteryEvents.OnPickingUpItem;
			Exiled.Events.Handlers.Player.Spawning -= flashlightBatteryEvents.OnSpawn;

			flashlightBatteryEvents = null;
		}

		internal static void AccessHintSystem(Player p, string hint, float time)
		{
			Loader.Plugins.FirstOrDefault(pl => pl.Name == "TipSystem")?.Assembly?.GetType("TipSystem.API.System")?.GetMethod("ShowHint", BindingFlags.Public | BindingFlags.Static)?.Invoke(null, new object[] { p, hint, time });
		}

		public override string Author => "Cyanox";
		public override string Name => "ExtraAdditions";
	}
}
