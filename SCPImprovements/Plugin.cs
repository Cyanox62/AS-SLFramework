using Exiled.API.Features;
using Exiled.Loader;
using HarmonyLib;
using System.Linq;
using System.Reflection;

namespace SCPImprovements
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

            hInstance = new Harmony("cyan.scpimprovements");
            hInstance.PatchAll();

            ev = new EventHandlers();

            Exiled.Events.Handlers.Player.Dying += ev.OnPlayerDeath;
            Exiled.Events.Handlers.Player.ChangingRole += ev.OnChangeRole;

            Exiled.Events.Handlers.Scp106.Containing += ev.OnScp106Contain;
            Exiled.Events.Handlers.Scp096.AddingTarget += ev.OnAddTarget;
            Exiled.Events.Handlers.Scp096.CalmingDown += ev.OnEndRage;

            Exiled.Events.Handlers.Cassie.SendingCassieMessage += ev.OnCassie;

            Exiled.Events.Handlers.Warhead.Detonated += ev.OnDetonated;

            Exiled.Events.Handlers.Server.RoundStarted += ev.OnRoundStart;
        }

        public override void OnDisabled()
        {
            base.OnDisabled();

            hInstance.UnpatchAll(hInstance.Id);
            hInstance = null;

            Exiled.Events.Handlers.Player.Dying -= ev.OnPlayerDeath;
            Exiled.Events.Handlers.Player.ChangingRole -= ev.OnChangeRole;

            Exiled.Events.Handlers.Scp106.Containing -= ev.OnScp106Contain;
            Exiled.Events.Handlers.Scp096.AddingTarget -= ev.OnAddTarget;
            Exiled.Events.Handlers.Scp096.CalmingDown -= ev.OnEndRage;

            Exiled.Events.Handlers.Cassie.SendingCassieMessage -= ev.OnCassie;

            Exiled.Events.Handlers.Warhead.Detonated -= ev.OnDetonated;

            Exiled.Events.Handlers.Server.RoundStarted -= ev.OnRoundStart;

            ev = null;
        }

        internal static void AccessHintSystem(Player p, string hint, float time)
        {
            Loader.Plugins.FirstOrDefault(pl => pl.Name == "TipSystem")?.Assembly?.GetType("TipSystem.API.System")?.GetMethod("ShowHint", BindingFlags.Public | BindingFlags.Static)?.Invoke(null, new object[] { p, hint, time });
        }

        public override string Author => "Cyanox";
        public override string Name => "SCPImprovements";
    }
}
