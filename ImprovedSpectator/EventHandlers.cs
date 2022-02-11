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

				//if (Respawn.IsSpawning) continue;

				string min = (Respawn.TimeUntilRespawn / 60).ToString();
				int sec = Respawn.TimeUntilRespawn % 60;
				string ssec = string.Empty;
				if (sec < 10) ssec += $"0{sec}";
				else ssec += sec;

				string nextTeam = "Unknown";
				if (Respawn.NextKnownTeam == SpawnableTeamType.NineTailedFox) nextTeam = Plugin.singleton.Translation.MTF;
				else if (Respawn.NextKnownTeam == SpawnableTeamType.ChaosInsurgency) nextTeam = Plugin.singleton.Translation.CI;

				string s = $"{Plugin.singleton.Translation.RespawnTime} {Plugin.singleton.Translation.Minutes.Replace("{minutes}", min)}{Plugin.singleton.Translation.Seconds.Replace("{seconds}", ssec)}";
				if (Respawn.NextKnownTeam != SpawnableTeamType.None) s += $"{Plugin.singleton.Translation.RespawnTeam} {nextTeam}";

				foreach (Player player in Player.List.Where(x => x.Team == Team.RIP))
				{
					player.ShowHint(s, 2f);
				}
			}
		}
	}
}
