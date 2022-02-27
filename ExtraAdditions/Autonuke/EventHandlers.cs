using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using System.Collections.Generic;

namespace ExtraAdditions.Autonuke
{
	class EventHandlers
	{
		private CoroutineHandle coroutine;

		internal void OnRoundStart()
		{
			coroutine = Timing.RunCoroutine(Autonuke());
		}

		internal void OnRoundEnd(RoundEndedEventArgs ev)
		{
			if (coroutine.IsRunning) Timing.KillCoroutines(coroutine);
		}

		private IEnumerator<float> Autonuke()
		{
			yield return Timing.WaitForSeconds(Plugin.singleton.Config.TimeUntilAutonuke);
			Warhead.Start();
			Warhead.IsLocked = !Plugin.singleton.Config.CanStopAutonuke;
		}
	}
}
