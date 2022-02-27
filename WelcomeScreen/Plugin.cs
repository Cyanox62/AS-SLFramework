using Exiled.API.Features;
using Exiled.Loader;
using System.Linq;
using System.Reflection;

namespace WelcomeScreen
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

            Exiled.Events.Handlers.Player.Verified += ev.OnPlayerVerified;

            Exiled.Events.Handlers.Server.RoundStarted += ev.OnRoundStart;
            Exiled.Events.Handlers.Server.WaitingForPlayers += ev.OnWaitingForPlayers;
		}

        public override void OnDisabled()
		{
            base.OnDisabled();

            Exiled.Events.Handlers.Player.Verified -= ev.OnPlayerVerified;

            Exiled.Events.Handlers.Server.RoundStarted -= ev.OnRoundStart;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= ev.OnWaitingForPlayers;

            ev = null;
        }

        internal static void AccessHintSystem(Player p, string hint, float time)
        {
            Loader.Plugins.FirstOrDefault(pl => pl.Name == "TipSystem")?.Assembly?.GetType("TipSystem.API.System")?.GetMethod("ShowHint", BindingFlags.Public | BindingFlags.Static)?.Invoke(null, new object[] { p, hint, time });
        }

        internal static void ClearHints(Player p, string filter = "")
        {
            Loader.Plugins.FirstOrDefault(pl => pl.Name == "TipSystem")?.Assembly?.GetType("TipSystem.API.System")?.GetMethod("ClearHints", BindingFlags.Public | BindingFlags.Static)?.Invoke(null, new object[] { p, filter });
        }

        public override string Author => "Cyanox";
        public override string Name => "WelcomeScreen";
    }
}
