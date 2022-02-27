using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using System.Collections.Generic;
using System.Linq;

namespace ExtraAdditions.ElevatorFailure
{
	class EventHandlers
	{
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
				Plugin.AccessHintSystem(ev.Player, $"{new string('\n', Plugin.singleton.Config.ElevatorHintTextLower)}{Plugin.singleton.Translation.BrokenElevator}", Plugin.singleton.Config.BrokenElevatorHintTime);
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
