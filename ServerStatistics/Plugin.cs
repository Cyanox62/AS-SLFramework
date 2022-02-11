using Exiled.API.Features;
using HarmonyLib;

namespace ServerStatistics
{
	public class Plugin : Plugin<Config, Translation>
	{
		internal static Plugin singleton;

		private Harmony hInstance;

		private EventHandlers ev;

		public override void OnEnabled()
		{
			base.OnEnabled();

			hInstance = new Harmony("cyan.serverstatistics");
			hInstance.PatchAll();

			ev = new EventHandlers();
			Exiled.Events.Handlers.Server.RoundStarted += ev.OnRoundStart;
			Exiled.Events.Handlers.Server.RoundEnded += ev.OnRoundEnd;
			Exiled.Events.Handlers.Server.RestartingRound += ev.OnRoundRestart;
			Exiled.Events.Handlers.Server.WaitingForPlayers += ev.OnWaitingForPlayers;
			Exiled.Events.Handlers.Server.RespawningTeam += ev.OnTeamRespawn;

			Exiled.Events.Handlers.Warhead.Starting += ev.OnNukeStart;
			Exiled.Events.Handlers.Warhead.Stopping += ev.OnNukeStop;
			Exiled.Events.Handlers.Warhead.Detonated += ev.OnNukeDetonate;

			Exiled.Events.Handlers.Map.Decontaminating += ev.OnDecontamination;

			singleton = this;
		}

		public override void OnDisabled()
		{
			base.OnDisabled();

			Exiled.Events.Handlers.Server.RoundStarted -= ev.OnRoundStart;
			Exiled.Events.Handlers.Server.RoundEnded -= ev.OnRoundEnd;
			Exiled.Events.Handlers.Server.RestartingRound -= ev.OnRoundRestart;
			Exiled.Events.Handlers.Server.WaitingForPlayers -= ev.OnWaitingForPlayers;
			Exiled.Events.Handlers.Server.RespawningTeam -= ev.OnTeamRespawn;

			Exiled.Events.Handlers.Warhead.Starting -= ev.OnNukeStart;
			Exiled.Events.Handlers.Warhead.Stopping -= ev.OnNukeStop;
			Exiled.Events.Handlers.Warhead.Detonated -= ev.OnNukeDetonate;

			Exiled.Events.Handlers.Map.Decontaminating -= ev.OnDecontamination;

			ev = null;

			hInstance.UnpatchAll(hInstance.Id);
			hInstance = null;
		}

		public override string Author => "Cyanox";
		public override string Name => "ServerStatistics";
	}
}
