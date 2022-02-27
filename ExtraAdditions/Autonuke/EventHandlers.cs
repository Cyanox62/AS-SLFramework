using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using System.Collections.Generic;

namespace ExtraAdditions.Autonuke
{
	class EventHandlers
	{
		private CoroutineHandle autoNukeCoroutine;
		private CoroutineHandle warheadTimerCoroutine;
		private List<CoroutineHandle> announcementCoroutines = new List<CoroutineHandle>();

		internal void OnRoundStart()
		{
			autoNukeCoroutine = Timing.RunCoroutine(Autonuke());
		}

		internal void OnRoundEnd(RoundEndedEventArgs ev)
		{
			if (autoNukeCoroutine.IsRunning) Timing.KillCoroutines(autoNukeCoroutine);
			if (warheadTimerCoroutine.IsRunning) Timing.KillCoroutines(warheadTimerCoroutine);
			Timing.KillCoroutines(announcementCoroutines.ToArray());
			announcementCoroutines.Clear();
		}

		internal void OnWarheadStart(StartingEventArgs ev)
		{
			warheadTimerCoroutine = Timing.RunCoroutine(WarheadTimer());
		}

		internal void OnWarheadStop(StoppingEventArgs ev)
		{
			if (warheadTimerCoroutine.IsRunning) Timing.KillCoroutines(warheadTimerCoroutine);
		}

		private IEnumerator<float> WarheadTimer()
		{
			while (Warhead.DetonationTimer > 0f)
			{
				yield return Timing.WaitForSeconds(1f);
				foreach (Player player in Player.List)
				{
					Plugin.AccessHintSystem(player, $"{new string('\n', Plugin.singleton.Config.WarheadHintTextLower)}{Plugin.singleton.Translation.WarheadDetonation.Replace("{seconds}", ((int)Warhead.DetonationTimer).ToString())}", 1f);
				}
			}
		}

		private IEnumerator<float> Autonuke()
		{
			yield return Timing.WaitForSeconds(Plugin.singleton.Config.TimeUntilAutonuke);
			Warhead.Start();
			Warhead.IsLocked = !Plugin.singleton.Config.CanStopAutonuke;
			foreach (var entry in Plugin.singleton.Config.CassieNukeAnnouncements)
			{
				announcementCoroutines.Add(Timing.RunCoroutine(CassieCoroutine(entry.Key, entry.Value)));
			}
		}

		private IEnumerator<float> CassieCoroutine(float delay, string cassie)
		{
			yield return Timing.WaitForSeconds(delay);
			Cassie.Message(cassie);
		}
	}
}
