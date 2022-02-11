using Exiled.API.Features;

namespace ImprovedSpectator
{
    public class Plugin : Plugin<Config, Translation>
    {
        internal static Plugin singleton;

        private EventHandlers ev;

        public override void OnEnabled()
        {
            base.OnEnabled();

            singleton = this;

            ev = new EventHandlers();

            Exiled.Events.Handlers.Server.RoundStarted += ev.OnRoundStart;
            Exiled.Events.Handlers.Server.RespawningTeam += ev.OnRespawnTeam;

            Exiled.Events.Handlers.Player.Spawning += ev.OnSpawn;
            Exiled.Events.Handlers.Player.Dying += ev.OnDeath;
        }

        public override void OnDisabled()
        {
            base.OnDisabled();

            Exiled.Events.Handlers.Server.RoundStarted -= ev.OnRoundStart;
            Exiled.Events.Handlers.Server.RespawningTeam -= ev.OnRespawnTeam;

            Exiled.Events.Handlers.Player.Spawning -= ev.OnSpawn;
            Exiled.Events.Handlers.Player.Dying -= ev.OnDeath;

            ev = null;
        }

        public override string Author => "Cyanox";
    }
}
