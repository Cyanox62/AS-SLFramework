using Exiled.API.Features;
using Exiled.Events.EventArgs;
using LightContainmentZoneDecontamination;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LightContainmentZoneDecontamination.DecontaminationController;

namespace ExtraAdditions.Misc
{
	class EventHandlers
	{
		private const int DecontaminationTime = 705;

		private CoroutineHandle warheadTimerCoroutine;
		private CoroutineHandle decontCoroutine;

		internal void OnRoundStart()
		{
			decontCoroutine = Timing.RunCoroutine(Decontamination());
		}

		internal void OnRestartingRound()
		{
			if (warheadTimerCoroutine.IsRunning) Timing.KillCoroutines(warheadTimerCoroutine);
			if (decontCoroutine.IsRunning) Timing.KillCoroutines(decontCoroutine);
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
			foreach (Player player in Player.List)
			{
				Plugin.AccessHintSystem(player, $"{new string('\n', Plugin.singleton.Config.DecontaminationHintTextLower)}{Plugin.singleton.Translation.Decontamination.Replace("{seconds}", i.ToString())}", 1f);
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
