using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using Respawning;
using Respawning.NamingRules;
using System.Collections.Generic;

namespace ExtraAdditions.Misc
{
	class EventHandlers
	{
		private const int DecontaminationTime = 705;

		private CoroutineHandle warheadTimerCoroutine;
		private CoroutineHandle decontCoroutine;
		private CoroutineHandle intercomCoroutine;
		private bool isCassieInUse = false;

		internal void OnRoundStart()
		{
			isCassieInUse = false;
			decontCoroutine = Timing.RunCoroutine(Decontamination());

			RespawnManager.Singleton.NamingManager.AllUnitNames.Add(
				new SyncUnit
				{
					UnitName = Plugin.singleton.Translation.UnitMessage,
					SpawnableTeam = (byte)SpawnableTeamType.NineTailedFox
				}
			);

			foreach (var entry in Plugin.singleton.Config.RoomColorOverrides)
			{
				foreach (Room room in Room.Get(x => x.Type == entry.Key))
				{
					room.Color = new UnityEngine.Color32(entry.Value[0], entry.Value[1], entry.Value[2], entry.Value[3]);
				}
			}
		}

		internal void OnRestartingRound()
		{
			if (warheadTimerCoroutine.IsRunning) Timing.KillCoroutines(warheadTimerCoroutine);
			if (decontCoroutine.IsRunning) Timing.KillCoroutines(decontCoroutine);
			if (intercomCoroutine.IsRunning) Timing.KillCoroutines(intercomCoroutine);
		}

		internal void OnIntercom(IntercomSpeakingEventArgs ev) => ev.IsAllowed = !isCassieInUse;

		internal void OnCassie(SendingCassieMessageEventArgs ev)
		{
			isCassieInUse = true;
			Intercom.host.CustomContent = Plugin.singleton.Translation.CassieInUse;
			intercomCoroutine = Timing.CallDelayed(6.3f + Cassie.CalculateDuration(ev.Words), () =>
			{
				isCassieInUse = false;
				Intercom.host.CustomContent = string.Empty;
			});
		}

		internal void OnWarheadStart(StartingEventArgs ev)
		{
			warheadTimerCoroutine = Timing.RunCoroutine(WarheadTimer());
		}

		internal void OnWarheadStop(StoppingEventArgs ev)
		{
			if (warheadTimerCoroutine.IsRunning) Timing.KillCoroutines(warheadTimerCoroutine);
		}

		private IEnumerator<float> Decontamination()
		{
			yield return Timing.WaitForSeconds(DecontaminationTime - 30);
			for (int i = 30; i > 0; i--)
			{
				yield return Timing.WaitForSeconds(1f);
				foreach (Player player in Player.List)
				{
					Plugin.AccessHintSystem(player, $"{new string('\n', Plugin.singleton.Config.DecontaminationHintTextLower)}{Plugin.singleton.Translation.Decontamination.Replace("{seconds}", i.ToString())}", 1f);
				}
			}
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
	}
}
