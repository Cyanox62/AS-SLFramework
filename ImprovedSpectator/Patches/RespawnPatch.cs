using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using NorthwoodLib.Pools;
using Respawning;
using UnityEngine;
using Exiled.API.Features;

namespace ImprovedSpectator.Patches
{
	[HarmonyPatch(typeof(RespawnManager), nameof(RespawnManager.Spawn))]
	class RespawnPatch
	{
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
			foreach (Player player in EventHandlers.additionalRespawnPlayers) list.Add(player.ReferenceHub);

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

	[HarmonyPatch(typeof(CharacterClassManager), nameof(CharacterClassManager.DeathTime), MethodType.Setter)]
	class DeathTimePatch
	{
		public static bool Prefix(CharacterClassManager __instance)
		{
			if (EventHandlers.additionalRespawnPlayers.Select(x => x.ReferenceHub).Contains(__instance._hub)) return false;
			else return true;
		}
	}
}
