using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs;
using Interactables.Interobjects.DoorUtils;
using MEC;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ExtraAdditions
{
	class EventHandlers
	{
		// Thanks Beryl <3
		// https://github.com/SebasCapo/RemoteKeycard
		internal void OnDoorAccess(InteractingDoorEventArgs ev)
		{
			if (Plugin.singleton.Config.RequireHeldKeycard) return;

			if (!ev.IsAllowed && ev.Player.Items.Any(item => item is Keycard keycard && (keycard.Base.Permissions & ev.Door.RequiredPermissions.RequiredPermissions) != 0))
			{
				ev.IsAllowed = true;
			}
		}

		internal void OnLockerAccess(InteractingLockerEventArgs ev)
		{
			if (Plugin.singleton.Config.RequireHeldKeycard) return;

			if (!ev.IsAllowed && ev.Chamber != null && ev.Player.Items.Any(item => item is Keycard keycard && keycard.Base.Permissions.HasFlagFast(ev.Chamber.RequiredPermissions)))
			{
				ev.IsAllowed = true;
			}
		}

		internal void OnGeneratorUnlock(UnlockingGeneratorEventArgs ev)
		{
			if (Plugin.singleton.Config.RequireHeldKeycard) return;

			if (!ev.IsAllowed && ev.Player.Items.Any(item => item is Keycard keycard && (keycard.Base.Permissions & ev.Generator.Base._requiredPermission) != 0))
			{
				ev.IsAllowed = true;
			}
		}

		// --- //

		private Exiled.API.Features.Lift lastBrokenLift = null;
		private Exiled.API.Features.Lift curBrokenLift = null;
		private CoroutineHandle liftCoroutine;

		internal void OnRoundStart() => liftCoroutine = Timing.RunCoroutine(ElevatorCoroutine());
		internal void OnRoundRestart()
		{
			if (liftCoroutine.IsRunning) Timing.KillCoroutines(liftCoroutine);
			lastBrokenLift = null;
			curBrokenLift = null;
		}

		internal void OnElevatorUse(InteractingElevatorEventArgs ev)
		{
			if (ev.Lift == curBrokenLift)
			{
				ev.IsAllowed = false;
				Plugin.AccessHintSystem(ev.Player, $"{new string('\n', Plugin.singleton.Config.TextLower)}{Plugin.singleton.Translation.BrokenElevator}", Plugin.singleton.Config.BrokenElevatorHintTime);
			}
		}

		private IEnumerator<float> ElevatorCoroutine()
		{
			while (Round.IsStarted)
			{
				yield return Timing.WaitForSeconds(UnityEngine.Random.Range(Plugin.singleton.Config.MinTimeBetweenElevatorFails, Plugin.singleton.Config.MaxTimeBetweenElevatorFails));
				IEnumerable<Exiled.API.Features.Lift> lifts = Exiled.API.Features.Lift.List.Where(x => x != lastBrokenLift);
				Exiled.API.Features.Lift lift = lifts.ElementAt(UnityEngine.Random.Range(0, lifts.Count()));
				curBrokenLift = lift;
				Log.Warn(curBrokenLift.Type);
				curBrokenLift.IsLocked = true;
				yield return Timing.WaitForSeconds(UnityEngine.Random.Range(Plugin.singleton.Config.MinElevatorFailTime, Plugin.singleton.Config.MaxElevatorFailTime));
				curBrokenLift.IsLocked = false;
				curBrokenLift = null;
				lastBrokenLift = curBrokenLift;
			}
		}
	}
}
