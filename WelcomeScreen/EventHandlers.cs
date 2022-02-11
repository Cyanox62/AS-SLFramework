using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using System.Collections.Generic;

namespace WelcomeScreen
{
	class EventHandlers
	{
		private List<Player> hintPlayers = new List<Player>();

		private CoroutineHandle coroutine;

		internal void OnWaitingForPlayers() => coroutine = Timing.RunCoroutine(HintCoroutine());

		internal void OnRoundStart()
		{
			Timing.KillCoroutines(coroutine);
			hintPlayers.Clear();
		}

		internal void OnPlayerVerified(VerifiedEventArgs ev) => hintPlayers.Add(ev.Player);

		private IEnumerator<float> HintCoroutine()
		{
			while (!Round.IsStarted)
			{
				foreach (Player player in hintPlayers)
				{
					player.ShowHint(Plugin.singleton.Config.Hint.Replace("{serverNum}", Plugin.singleton.Config.ServerNumber.ToString()), 2f);
				}
				yield return Timing.WaitForSeconds(1f);
			}
		}
	}
}
