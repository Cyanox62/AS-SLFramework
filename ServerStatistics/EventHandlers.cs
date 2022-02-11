using Exiled.API.Features;
using Exiled.Events.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerStatistics
{
	class EventHandlers
	{
		private void SendWebhook(string message)
		{
			using (dWebHook dcWeb = new dWebHook())
			{
				dcWeb.ProfilePicture = Plugin.singleton.Config.ServerEventAvatarURL;
				dcWeb.UserName = Plugin.singleton.Config.ServerEventName;
				dcWeb.WebHook = Plugin.singleton.Config.ServerEventWebhook;
				dcWeb.SendMessage(message);
			}
		}

		internal void OnRoundStart()
		{
			SendWebhook(Plugin.singleton.Translation.RoundStart
				.Replace("{playerCount}", Player.List.Count().ToString()));
		}

		internal void OnRoundEnd(RoundEndedEventArgs ev)
		{
			SendWebhook(Plugin.singleton.Translation.RoundEnd);
		}

		internal void OnRoundRestart()
		{
			SendWebhook(Plugin.singleton.Translation.RoundRestart);
		}

		internal void OnWaitingForPlayers()
		{
			SendWebhook(Plugin.singleton.Translation.WaitingForPlayers);
		}

		internal void OnNukeStart(StartingEventArgs ev)
		{
			SendWebhook(Plugin.singleton.Translation.NukeStart);
		}

		internal void OnNukeStop(StoppingEventArgs ev)
		{
			SendWebhook(Plugin.singleton.Translation.NukeStop);
		}

		internal void OnNukeDetonate()
		{
			SendWebhook(Plugin.singleton.Translation.NukeDetonate);
		}

		internal void OnDecontamination(DecontaminatingEventArgs ev)
		{
			SendWebhook(Plugin.singleton.Translation.Decontamination);
		}

		internal void OnTeamRespawn(RespawningTeamEventArgs ev)
		{
			SendWebhook(Plugin.singleton.Translation.TeamRespawn
				.Replace("{team}", ev.NextKnownTeam == Respawning.SpawnableTeamType.NineTailedFox ? "Nine-Tailed Fox" : "Chaos Insurgency"));
		}
	}
}
