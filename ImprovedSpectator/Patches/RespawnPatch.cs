using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using NorthwoodLib.Pools;
using Respawning;
using UnityEngine;
using Exiled.API.Features;
using ImprovedSpectator;

namespace ImprovedSpectator.Patches
{
	[HarmonyPatch(typeof(RespawnManager), nameof(RespawnManager.Spawn))]
	class RespawnPatch
	{
		[HarmonyPriority(420)]
		public static bool Prefix(RespawnManager __instance)
		{
			SpawnableTeamHandlerBase spawnableTeamHandlerBase;
			if (!RespawnWaveGenerator.SpawnableTeams.TryGetValue(__instance.NextKnownTeam, out spawnableTeamHandlerBase) || __instance.NextKnownTeam == SpawnableTeamType.None)
			{
				global::ServerConsole.AddLog("Fatal error. Team '" + __instance.NextKnownTeam + "' is undefined.", ConsoleColor.Red);
				return false;
			}
			List<global::ReferenceHub> list = (from item in global::ReferenceHub.GetAllHubs().Values
											   where item.characterClassManager.CurClass == global::RoleType.Spectator && !item.serverRoles.OverwatchEnabled
											   select item).ToList<global::ReferenceHub>();

			// Ensure spawned in players get added to the queue

			foreach (Player player in EventHandlers.additionalRespawnPlayers)
			{
				Log.Warn("adding: " + player.Nickname + " to respawn list");
				list.Add(player.ReferenceHub);
			}

			if (__instance._prioritySpawn)
			{
				list = (from item in list
						orderby item.characterClassManager.DeathTime
						select item).ToList<global::ReferenceHub>();
			}
			else
			{
				list.ShuffleList<global::ReferenceHub>();
			}
			int num = RespawnTickets.Singleton.GetAvailableTickets(__instance.NextKnownTeam);
			if (RespawnTickets.Singleton.IsFirstWave)
			{
				RespawnTickets.Singleton.IsFirstWave = false;
			}
			if (num == 0)
			{
				num = 5;
				RespawnTickets.Singleton.GrantTickets(SpawnableTeamType.ChaosInsurgency, 5, true);
			}
			int num2 = Mathf.Min(num, spawnableTeamHandlerBase.MaxWaveSize);
			while (list.Count > num2)
			{
				list.RemoveAt(list.Count - 1);
			}
			list.ShuffleList<global::ReferenceHub>();
			List<global::ReferenceHub> list2 = ListPool<global::ReferenceHub>.Shared.Rent();
			Queue<global::RoleType> queue = new Queue<global::RoleType>();
			spawnableTeamHandlerBase.GenerateQueue(queue, list.Count);
			foreach (global::ReferenceHub referenceHub in list)
			{
				try
				{
					global::RoleType classid = queue.Dequeue();
					referenceHub.characterClassManager.SetPlayersClass(classid, referenceHub.gameObject, global::CharacterClassManager.SpawnReason.Respawn, false);
					list2.Add(referenceHub);
					global::ServerLogs.AddLog(global::ServerLogs.Modules.ClassChange, string.Concat(new string[]
					{
						"Player ",
						referenceHub.LoggedNameFromRefHub(),
						" respawned as ",
						classid.ToString(),
						"."
					}), global::ServerLogs.ServerLogType.GameEvent, false);
					Player p = Player.Get(referenceHub);
					if (p != null)
					{
						if (EventHandlers.additionalRespawnPlayers.Contains(p)) EventHandlers.additionalRespawnPlayers.Remove(p);
						if (EventHandlers.ghostPlayers.Contains(p)) EventHandlers.RemoveGhostPlayer(p);
					}
				}
				catch (Exception ex)
				{
					if (referenceHub != null)
					{
						global::ServerLogs.AddLog(global::ServerLogs.Modules.ClassChange, "Player " + referenceHub.LoggedNameFromRefHub() + " couldn't be spawned. Err msg: " + ex.Message, global::ServerLogs.ServerLogType.GameEvent, false);
					}
					else
					{
						global::ServerLogs.AddLog(global::ServerLogs.Modules.ClassChange, "Couldn't spawn a player - target's ReferenceHub is null.", global::ServerLogs.ServerLogType.GameEvent, false);
					}
				}
			}
			if (list2.Count > 0)
			{
				global::ServerLogs.AddLog(global::ServerLogs.Modules.ClassChange, string.Concat(new object[]
				{
					"RespawnManager has successfully spawned ",
					list2.Count,
					" players as ",
					__instance.NextKnownTeam.ToString(),
					"!"
				}), global::ServerLogs.ServerLogType.GameEvent, false);
				RespawnTickets.Singleton.GrantTickets(__instance.NextKnownTeam, -list2.Count * spawnableTeamHandlerBase.TicketRespawnCost, false);
				Respawning.NamingRules.UnitNamingRule unitNamingRule;
				if (Respawning.NamingRules.UnitNamingRules.TryGetNamingRule(__instance.NextKnownTeam, out unitNamingRule))
				{
					string text;
					unitNamingRule.GenerateNew(__instance.NextKnownTeam, out text);
					foreach (global::ReferenceHub referenceHub2 in list2)
					{
						referenceHub2.characterClassManager.NetworkCurSpawnableTeamType = (byte)__instance.NextKnownTeam;
						referenceHub2.characterClassManager.NetworkCurUnitName = text;
					}
					unitNamingRule.PlayEntranceAnnouncement(text);
				}
				RespawnEffectsController.ExecuteAllEffects(RespawnEffectsController.EffectType.UponRespawn, __instance.NextKnownTeam);
			}
			ListPool<global::ReferenceHub>.Shared.Return(list2);
			__instance.NextKnownTeam = SpawnableTeamType.None;
			return false;
		}
	}

	[HarmonyPatch(typeof(RespawnTickets), nameof(RespawnTickets.DrawRandomTeam))]
	class RespawnPatch2
	{
		public static bool Prefix(RespawnTickets __instance, ref SpawnableTeamType __result)
		{
			bool flag = false;
			foreach (KeyValuePair<GameObject, global::ReferenceHub> keyValuePair in global::ReferenceHub.GetAllHubs())
			{
				if ((keyValuePair.Value.characterClassManager.CurClass == global::RoleType.Spectator && !keyValuePair.Value.serverRoles.OverwatchEnabled) ||
					(EventHandlers.additionalRespawnPlayers.Count > 0))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				__result = SpawnableTeamType.None;
				return false;
			}
			SpawnableTeamType result;
			if (__instance.IsFirstWave)
			{
				result = __instance.GetHighestTicketTeam();
			}
			else
			{
				List<SpawnableTeamType> list = ListPool<SpawnableTeamType>.Shared.Rent();
				List<SpawnableTeamType> list2 = ListPool<SpawnableTeamType>.Shared.Rent();
				using (Dictionary<SpawnableTeamType, int>.Enumerator enumerator2 = __instance._tickets.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						KeyValuePair<SpawnableTeamType, int> keyValuePair2 = enumerator2.Current;
						SpawnableTeamHandlerBase spawnableTeamHandlerBase;
						if (keyValuePair2.Value > 0)
						{
							for (int i = 0; i < keyValuePair2.Value; i++)
							{
								list.Add(keyValuePair2.Key);
							}
						}
						else if (keyValuePair2.Value == 0 && RespawnWaveGenerator.SpawnableTeams.TryGetValue(keyValuePair2.Key, out spawnableTeamHandlerBase) && spawnableTeamHandlerBase.LockUponZero)
						{
							list2.Add(keyValuePair2.Key);
						}
					}
					goto IL_140;
				}
			IL_124:
				__instance._tickets[list2[0]] = -1;
				list2.RemoveAt(0);
			IL_140:
				if (list2.Count > 0)
				{
					goto IL_124;
				}
				result = ((list.Count == 0) ? SpawnableTeamType.ChaosInsurgency : list[UnityEngine.Random.Range(0, list.Count)]);
				ListPool<SpawnableTeamType>.Shared.Return(list);
				ListPool<SpawnableTeamType>.Shared.Return(list2);
			}
			__result = result;
			return false;
		}
	}
}
