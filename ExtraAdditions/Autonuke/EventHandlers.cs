using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using System.Collections.Generic;

namespace ExtraAdditions.Autonuke
{
	class EventHandlers
	{
		private CoroutineHandle coroutine;
		private List<CoroutineHandle> announcementCoroutines = new List<CoroutineHandle>();

		internal void OnRoundStart()
		{
			coroutine = Timing.RunCoroutine(Autonuke());
		}

		internal void OnRoundEnd(RoundEndedEventArgs ev)
		{
			if (coroutine.IsRunning) Timing.KillCoroutines(coroutine);
			Timing.KillCoroutines(announcementCoroutines.ToArray());
			announcementCoroutines.Clear();
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
