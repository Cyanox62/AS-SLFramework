using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using Respawning;
using System.Collections.Generic;
using System.Linq;

namespace ImprovedSpectator
{
	class EventHandlers
	{
		internal static List<Player> ghostPlayers = new List<Player>();
		internal static List<Player> additionalRespawnPlayers = new List<Player>();

		internal void OnRoundStart()
		{
			Timing.RunCoroutine(RespawnTimerCoroutine());
			ghostPlayers.Clear();
			additionalRespawnPlayers.Clear();
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
			if (!additionalRespawnPlayers.Contains(ev.Player)) ev.Player.ShowHint(string.Empty, float.MaxValue);
			foreach (Player player in ghostPlayers)
			{
				if (!ev.Player.TargetGhostsHashSet.Contains(player.Id))
				{
					ev.Player.TargetGhostsHashSet.Add(player.Id);
				}
			}
		}

		internal void OnDoorAccess(InteractingDoorEventArgs ev)
		{
			if (ghostPlayers.Contains(ev.Player)) ev.IsAllowed = false;
		}

		internal void OnElevatorAccess(InteractingElevatorEventArgs ev)
		{
			if (ghostPlayers.Contains(ev.Player)) ev.IsAllowed = false;
		}

		internal void OnLockerAccess(InteractingLockerEventArgs ev)
		{
			if (ghostPlayers.Contains(ev.Player)) ev.IsAllowed = false;
		}

		internal void OnIntercomAccess(IntercomSpeakingEventArgs ev)
		{
			if (ghostPlayers.Contains(ev.Player)) ev.IsAllowed = false;
		}

		internal void OnScp330Access(InteractingScp330EventArgs ev)
		{
			if (ghostPlayers.Contains(ev.Player)) ev.IsAllowed = false;
		}

		internal void OnSetRole(ChangingRoleEventArgs ev)
		{
			if (additionalRespawnPlayers.Contains(ev.Player))
			{
				additionalRespawnPlayers.Remove(ev.Player);
			}
			if (ghostPlayers.Contains(ev.Player))
			{
				RemoveGhostPlayer(ev.Player);
			}
		}

		internal void OnPickupItem(PickingUpItemEventArgs ev)
		{
			if (ghostPlayers.Contains(ev.Player))
			{
				ev.IsAllowed = false;
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

				foreach (Player player in Player.List.Where(x => x.Team == Team.RIP || additionalRespawnPlayers.Contains(x)))
				{
					player.ShowHint(s, 2f);
				}
			}
		}
	}
}
