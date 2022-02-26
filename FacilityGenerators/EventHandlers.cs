using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Items;
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

			List<Room> rooms = Room.Get(x => x.Zone != ZoneType.Surface).ToList();
			List<Room> discard = new List<Room>();
			for (int i = 0; i < Plugin.singleton.Config.FlashlightsToSpawn; i++)
			{
				if (i % (rooms.Count + discard.Count) == 0)
				{
					foreach (Room room in discard) rooms.Add(room);
					discard.Clear();
					rooms.ShuffleList();
				}
				Room chosenRoom = rooms.First();
				Vector3 pos = chosenRoom.Position;
				pos.y += 2;
				Item.Create(ItemType.Flashlight).Spawn(pos, Quaternion.Euler(UnityEngine.Random.Range(10f, 80f), UnityEngine.Random.Range(10f, 80f), UnityEngine.Random.Range(10f, 80f)));
				discard.Add(chosenRoom);
				rooms.RemoveAt(0);
			}
		}

		internal void OnRoundRestart()
		{
			if (coroutine.IsRunning) Timing.KillCoroutines(coroutine);
		}

		internal void OnSpawn(SpawningEventArgs ev)
		{
			if (ev.Player.Role.Team == Team.MTF || ev.Player.Role.Team == Team.RSC)
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
				foreach (Room room in Room.List)
				{
					room.FlickerableLightController.ServerFlickerLights(0f);
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
				foreach (Room room in Room.List)
				{
					room.FlickerableLightController.ServerFlickerLights(float.MaxValue);
				}
			}
		}

		internal void OnNukeDetonate() => isWarheadStarted = false;

		private IEnumerator<float> BlackoutCoroutine()
		{
			int totalBlackouts = UnityEngine.Random.Range(Plugin.singleton.Config.MinBlackoutsPerRound, Plugin.singleton.Config.MaxBlackoutsPerRound);
			Log("Total blackouts planned for this round: " + totalBlackouts);
			for (int i = 0; i < totalBlackouts; i++)
			//for (int i = 0; i < 2; i++)
			{
				float randomDelay = UnityEngine.Random.Range(Plugin.singleton.Config.MinTimeBetweenBlackouts, Plugin.singleton.Config.MaxTimeBetweenBlackouts);
				Log("Next blackout will happen in: " + randomDelay + " seconds");
				yield return Timing.WaitForSeconds(randomDelay);
				//yield return Timing.WaitForSeconds(10f);
				if (EventHandlers.isWarheadDetonated) yield break;
				else
				{
					Cassie.Message(Plugin.singleton.Config.CassieBlackoutStart, false);

					Log("Beginning CASSIE start announcement");

					yield return Timing.WaitForSeconds(Plugin.singleton.Config.CassieBlackoutStartOffset);

					isBlackout = true;

					Log("Blackout starting");

					float dur = UnityEngine.Random.Range(Plugin.singleton.Config.MinBlackoutDuration, Plugin.singleton.Config.MaxBlackoutDuration);
					//foreach (FlickerableLightController controller in FlickerableLightController.Instances)
					//{
					//	controller.ServerFlickerLights(0.1f);
					//}
					//yield return Timing.WaitForSeconds(1f);
					foreach (Room room in Room.List)
					{
						if (EventHandlers.isWarheadStarted)
						{
							room.Color = Color.red;
							room.LightIntensity = 0.3f;
						}
						else
						{
							room.FlickerableLightController.ServerFlickerLights(float.MaxValue);
						}
					}
					isBlackout = true;
					Log("Blackout will remain for: " + dur + " seconds");
					yield return Timing.WaitForSeconds(dur);

					Cassie.Message(Plugin.singleton.Config.CassieBlackoutEnd, false);

					Log("Beginning CASSIE end announcement");

					yield return Timing.WaitForSeconds(Plugin.singleton.Config.CassieBlackoutEndOffset);

					Log("Ending blackout");

					//foreach (FlickerableLightController controller in FlickerableLightController.Instances)
					//{
					//	controller.ServerFlickerLights(0.1f);
					//}
					//yield return Timing.WaitForSeconds(0.05f);
					foreach (Room room in Room.List)
					{
						room.FlickerableLightController.ServerFlickerLights(0f);
						if (isWarheadStarted)
						{
							room.Color = defaultColor;
							room.LightIntensity = 1f;
							room.FlickerableLightController.WarheadLightOverride = false;
						}
					}
					foreach (Exiled.API.Features.TeslaGate tesla in Exiled.API.Features.TeslaGate.List)
					{
						tesla.Trigger();
					}
					isBlackout = false;
				}
			}
		}

		private void Log(string msg)
		{
			if (Plugin.singleton.Config.DebugMode) Exiled.API.Features.Log.Info(msg);
		}
	}
}
