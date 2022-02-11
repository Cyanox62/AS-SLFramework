using Exiled.API.Features;
using HarmonyLib;

namespace ImprovedSpectator
{
    public class Plugin : Plugin<Config, Translation>
    {
        internal static Plugin singleton;

        private EventHandlers ev;

        private Harmony hInstance;

        public override void OnEnabled()
        {
            base.OnEnabled();

            singleton = this;

            hInstance = new Harmony("cyan.improvedspectator");
            hInstance.PatchAll();

            ev = new EventHandlers();

            Exiled.Events.Handlers.Server.RoundStarted += ev.OnRoundStart;
            Exiled.Events.Handlers.Server.RespawningTeam += ev.OnRespawnTeam;

            Exiled.Events.Handlers.Player.Spawning += ev.OnSpawn;
            Exiled.Events.Handlers.Player.Dying += ev.OnDeath;
            Exiled.Events.Handlers.Player.PickingUpItem += ev.OnPickupItem;
            Exiled.Events.Handlers.Player.InteractingDoor += ev.OnDoorAccess;
            Exiled.Events.Handlers.Player.InteractingElevator += ev.OnElevatorAccess;
            Exiled.Events.Handlers.Player.InteractingLocker += ev.OnLockerAccess;
            Exiled.Events.Handlers.Player.InteractingScp330 += ev.OnScp330Access;
            Exiled.Events.Handlers.Player.IntercomSpeaking += ev.OnIntercomAccess;
            Exiled.Events.Handlers.Player.ChangingRole += ev.OnSetRole;
        }

        public override void OnDisabled()
        {
            base.OnDisabled();

            Exiled.Events.Handlers.Server.RoundStarted -= ev.OnRoundStart;
            Exiled.Events.Handlers.Server.RespawningTeam -= ev.OnRespawnTeam;

            Exiled.Events.Handlers.Player.Spawning -= ev.OnSpawn;
            Exiled.Events.Handlers.Player.Dying -= ev.OnDeath;
            Exiled.Events.Handlers.Player.PickingUpItem -= ev.OnPickupItem;
            Exiled.Events.Handlers.Player.InteractingDoor -= ev.OnDoorAccess;
            Exiled.Events.Handlers.Player.InteractingElevator -= ev.OnElevatorAccess;
            Exiled.Events.Handlers.Player.InteractingLocker -= ev.OnLockerAccess;
            Exiled.Events.Handlers.Player.InteractingScp330 -= ev.OnScp330Access;
            Exiled.Events.Handlers.Player.IntercomSpeaking -= ev.OnIntercomAccess;
            Exiled.Events.Handlers.Player.ChangingRole -= ev.OnSetRole;

            ev = null;

            hInstance.UnpatchAll(hInstance.Id);
            hInstance = null;
        }

        public override string Author => "Cyanox";
        public override string Name => "ImprovedSpectator";
    }
}
