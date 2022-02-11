using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FacilityGenerators
{
	class EventHandlers
	{
		internal static bool isWarheadStarted = false;
		internal static bool isWarheadDetonated = false;
		internal static bool isBlackout = false;

		private Color defaultColor = new Color(1f, 0.2f, 0.2f, 1f);

		private CoroutineHandle coroutine;

		internal void OnRoundStart()
		{
			isBlackout = false;

			coroutine = Timing.RunCoroutine(BlackoutCoroutine());
		}

		internal void OnRoundRestart()
		{
			if (coroutine.IsRunning) Timing.KillCoroutines(coroutine);
		}

		internal void OnSpawn(SpawningEventArgs ev)
		{
			if (ev.Player.Team == Team.MTF)
			{
				ev.Player.AddItem(ItemType.Flashlight);
			}
		}

		internal void OnTesla(TriggeringTeslaEventArgs ev)
		{
			if (isBlackout) ev.IsTriggerable = false;
		}

		internal void OnNukeStart(StartingEventArgs ev)
		{
			isWarheadStarted = true;
			if (isBlackout)
			{
				foreach (Room room in Map.Rooms)
				{
					room.Color = Color.red;
					room.LightIntensity = 0.3f;
				}
			}
		}

		internal void OnNukeStop(StoppingEventArgs ev)
		{
			isWarheadStarted = false;
			if (isBlackout)
			{
				foreach (Room room in Map.Rooms)
				{
					room.Color = Color.black;
					room.LightIntensity = 0.3f;
				}
			}
		}

		internal void OnNukeDetonate() => isWarheadStarted = false;

		private IEnumerator<float> BlackoutCoroutine()
		{
			for (int i = 0; i < UnityEngine.Random.Range(Plugin.singleton.Config.MinBlackoutsPerRound, Plugin.singleton.Config.MaxBlackoutsPerRound); i++)
			{
				yield return Timing.WaitForSeconds(UnityEngine.Random.Range(Plugin.singleton.Config.MinTimeBetweenBlackouts, Plugin.singleton.Config.MaxTimeBetweenBlackouts));
				if (EventHandlers.isWarheadDetonated) yield break;
				else
				{
					Cassie.Message(Plugin.singleton.Config.CassieBlackoutStart, false);
					yield return Timing.WaitForSeconds(Plugin.singleton.Config.CassieBlackoutStartOffset);

					isBlackout = true;

					float dur = UnityEngine.Random.Range(Plugin.singleton.Config.MinBlackoutDuration, Plugin.singleton.Config.MaxBlackoutDuration);
					foreach (FlickerableLightController controller in FlickerableLightController.Instances)
					{
						controller.ServerFlickerLights(0.1f);
					}
					yield return Timing.WaitForSeconds(1f);
					foreach (Room room in Map.Rooms)
					{
						if (EventHandlers.isWarheadStarted)
						{
							room.Color = Color.red;
							room.LightIntensity = 0.3f;
						}
						else
						{
							room.Color = Color.black;
						}
					}
					isBlackout = true;
					yield return Timing.WaitForSeconds(dur);

					Cassie.Message(Plugin.singleton.Config.CassieBlackoutEnd, false);

					yield return Timing.WaitForSeconds(Plugin.singleton.Config.CassieBlackoutEndOffset);

					foreach (FlickerableLightController controller in FlickerableLightController.Instances)
					{
						controller.ServerFlickerLights(0.1f);
					}
					yield return Timing.WaitForSeconds(0.05f);
					foreach (Room room in Map.Rooms)
					{
						room.Color = defaultColor;
						room.LightIntensity = 1f;
						room.FlickerableLightController.WarheadLightOverride = false;
					}
					foreach (TeslaGate tesla in Map.TeslaGates)
					{
						tesla.ServerSideCode();
					}
					isBlackout = false;
				}
			}
		}
	}
}
