using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using System.Collections.Generic;

namespace WelcomeScreen
{
	class EventHandlers
	{
		private CoroutineHandle coroutine;

		internal void OnWaitingForPlayers()
		{
			if (Plugin.singleton.Config.ShowHint) coroutine = Timing.RunCoroutine(HintCoroutine());
		}

		internal void OnRoundStart() => Timing.KillCoroutines(coroutine);

		internal void OnPlayerVerified(VerifiedEventArgs ev)
		{
			ev.Player.Broadcast((ushort)Plugin.singleton.Config.BroadcastTime, Plugin.singleton.Translation.WelcomeMessage);
		}

		private IEnumerator<float> HintCoroutine()
		{
			while (!Round.IsStarted)
			{
				yield return Timing.WaitForSeconds(1f);

				foreach (Player player in Player.List)
				{
					player.ShowHint($"{Plugin.singleton.Translation.ServerNumberText.Insert(0, new string('\n', Plugin.singleton.Config.TextLower)).Replace("{serverNum}", Plugin.singleton.Config.ServerNumber.ToString())}\n{Plugin.singleton.Translation.DiscordLink}", 2f);
				}
			}
		}
	}
}
