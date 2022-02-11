using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using Respawning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImprovedSpectator
{
	class EventHandlers
	{
		internal static List<Player> ghostPlayers = new List<Player>();
		internal static List<Player> additionalRespawnPlayers = new List<Player>();

		internal void OnRoundStart()
		{
			Timing.RunCoroutine(RespawnTimerCoroutine());
		}

		internal void OnRespawnTeam(RespawningTeamEventArgs ev)
		{
			foreach (Player player in ev.Players)
			{
				if (additionalRespawnPlayers.Contains(player))
				{
					additionalRespawnPlayers.Remove(player);
				}
				if (ghostPlayers.Contains(player))
				{
					RemoveGhostPlayer(player);
				}
			}
		}

		internal void OnSpawn(SpawningEventArgs ev)
		{
			foreach (Player player in ghostPlayers)
			{
				if (!ev.Player.TargetGhostsHashSet.Contains(player.Id))
				{
					ev.Player.TargetGhostsHashSet.Add(player.Id);
				}
			}
		}

		internal void OnDeath(DyingEventArgs ev)
		{
			foreach (Player player in ghostPlayers)
			{
				if (ev.Target.TargetGhostsHashSet.Contains(player.Id))
				{
					ev.Target.TargetGhostsHashSet.Remove(player.Id);
				}
			}
		}

		internal static void AddGhostPlayer(Player player)
		{
			ghostPlayers.Add(player);
			foreach (Player p in Player.List.Where(x => x.IsAlive))
			{
				p.TargetGhostsHashSet.Add(player.Id);
			}
		}

		internal static void RemoveGhostPlayer(Player player)
		{
			ghostPlayers.Remove(player);
			foreach (Player p in Player.List)
			{
				if (p.TargetGhostsHashSet.Contains(player.Id))
				{
					p.TargetGhostsHashSet.Remove(player.Id);
				}
			}
		}

		private IEnumerator<float> RespawnTimerCoroutine()
		{
			while (Round.IsStarted)
			{
				yield return Timing.WaitForSeconds(1f);

				string min = (Respawn.TimeUntilRespawn / 60).ToString();
				int sec = Respawn.TimeUntilRespawn % 60;
				string ssec = string.Empty;
				if (sec < 10) ssec += $"0{sec}";
				else ssec += sec;

				string nextTeam = "Unknown";
				if (Respawn.NextKnownTeam == SpawnableTeamType.NineTailedFox) nextTeam = Plugin.singleton.Translation.MTF;
				else if (Respawn.NextKnownTeam == SpawnableTeamType.ChaosInsurgency) nextTeam = Plugin.singleton.Translation.CI;

				string s = $"{new string('\n', Plugin.singleton.Config.TextLower)}{Plugin.singleton.Translation.RespawnTime.Replace("{minutes}", min).Replace("{seconds}", ssec)}";
				if (Respawn.IsSpawning)
				{
					s += $"\n{Plugin.singleton.Translation.RespawnInProgress.Replace("{team}", nextTeam)}";
				}
				else if (Respawn.NextKnownTeam != SpawnableTeamType.None) s += $"\n{Plugin.singleton.Translation.RespawnTeam} {Plugin.singleton.Translation.RespawnTeam.Replace("{team}", nextTeam)}";

				foreach (Player player in Player.List.Where(x => x.Team == Team.RIP))
				{
					player.ShowHint(s, 2f);
				}
			}
		}
	}
}
