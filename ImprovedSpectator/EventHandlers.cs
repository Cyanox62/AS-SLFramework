using Exiled.API.Features;
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
		internal void OnRoundStart()
		{
			Timing.RunCoroutine(RespawnTimerCoroutine());
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
