using Exiled.API.Features;
using HarmonyLib;

namespace FacilityGenerators
{
    public class Plugin : Plugin<Config>
    {
        internal static Plugin singleton;

        private EventHandlers ev;

        private Harmony hInstance;

        public override void OnEnabled()
        {
            base.OnEnabled();

            singleton = this;

            hInstance = new Harmony("cyan.facilitygenerators");
            hInstance.PatchAll();

            ev = new EventHandlers();

            Exiled.Events.Handlers.Server.RoundStarted += ev.OnRoundStart;
            Exiled.Events.Handlers.Server.WaitingForPlayers += ev.OnWaitingForPlayers;

            Exiled.Events.Handlers.Player.Spawning += ev.OnSpawn;
            Exiled.Events.Handlers.Player.TriggeringTesla += ev.OnTesla;
        }

        public override void OnDisabled()
        {
            base.OnDisabled();

            Exiled.Events.Handlers.Server.RoundStarted -= ev.OnRoundStart;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= ev.OnWaitingForPlayers;

            Exiled.Events.Handlers.Player.Spawning -= ev.OnSpawn;
            Exiled.Events.Handlers.Player.TriggeringTesla -= ev.OnTesla;

            ev = null;

            hInstance.UnpatchAll(hInstance.Id);
            hInstance = null;
        }

        public override string Author => "Cyanox";
    }
}
