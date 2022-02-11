using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using System.Collections.Generic;

namespace WelcomeScreen
{
	class EventHandlers
	{
		private CoroutineHandle coroutine;

		internal void OnWaitingForPlayers() => coroutine = Timing.RunCoroutine(HintCoroutine());

		internal void OnRoundStart() => Timing.KillCoroutines(coroutine);

		private IEnumerator<float> HintCoroutine()
		{
			while (!Round.IsStarted)
			{
				foreach (Player player in Player.List)
				{
					player.ShowHint(Plugin.singleton.Config.Hint.Insert(0, new string('\n', Plugin.singleton.Config.TextLower)).Replace("{serverNum}", Plugin.singleton.Config.ServerNumber.ToString()), 2f);
				}
				yield return Timing.WaitForSeconds(1f);
			}
		}
	}
}
