using Exiled.API.Features;
using HarmonyLib;
using MEC;
using PlayableScps;
using System.Collections.Generic;
using UnityEngine;

namespace SCPImprovements.Patches
{
	class Scp096Vision
	{
		[HarmonyPatch(typeof(PlayableScps.Scp096), nameof(PlayableScps.Scp096.UpdateVision))]
		public bool Prefix(PlayableScps.Scp096 __instance)
		{
			Vector3 vector = __instance.Hub.transform.TransformPoint(PlayableScps.Scp096._headOffset);
			foreach (KeyValuePair<GameObject, global::ReferenceHub> keyValuePair in global::ReferenceHub.GetAllHubs())
			{
				global::ReferenceHub value = keyValuePair.Value;
				global::CharacterClassManager characterClassManager = value.characterClassManager;
				Player p = Player.Get(value);
				if (characterClassManager.CurClass != global::RoleType.Spectator && !(value == __instance.Hub) && !characterClassManager.IsAnyScp() && Vector3.Dot((value.PlayerCameraReference.position - vector).normalized, __instance.Hub.PlayerCameraReference.forward) >= 0.1f)
				{
					Plugin.AccessHintSystem(p, Plugin.singleton.Translation.CanSee096, 1f);
					if (!EventHandlers.isLookingAt096.Contains(p)) EventHandlers.isLookingAt096.Add(p);
					VisionInformation visionInformation = VisionInformation.GetVisionInformation(value, vector, -0.1f, 60f, true, true, __instance.Hub.localCurrentRoomEffects, 0);
					if (visionInformation.IsLooking)
					{
						float delay = visionInformation.LookingAmount / 0.25f * (visionInformation.Distance * 0.1f);
						if (!__instance.Calming)
						{
							if (__instance.CanReceiveTargets && !__instance._targets.Contains(value))
							{
								if (p != null) Plugin.AccessHintSystem(p, Plugin.singleton.Translation.TargetOf096, Plugin.singleton.Config.Scp096TargetHintTime);
							}
							__instance.AddTarget(value.gameObject);
						}
						if (__instance.CanEnrage && value.gameObject != null)
						{
							__instance.PreWindup(delay);
						}
					}
				}
				else
				{
					if (EventHandlers.isLookingAt096.Contains(p) && !__instance._targets.Contains(value))
					{
						EventHandlers.isLookingAt096.Remove(p);
						if (!EventHandlers.lookingAt096Cooldown.ContainsKey(p)) EventHandlers.lookingAt096Cooldown.Add(p, Timing.RunCoroutine(EventHandlers.StartSafeDelay(p)));
						else
						{
							Timing.KillCoroutines(EventHandlers.lookingAt096Cooldown[p]);
							EventHandlers.lookingAt096Cooldown[p] = Timing.RunCoroutine(EventHandlers.StartSafeDelay(p));
						}
					}
				}
			}

			return false;
		}
	}
}
