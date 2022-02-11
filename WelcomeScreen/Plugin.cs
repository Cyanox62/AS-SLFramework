using Exiled.API.Features;

namespace WelcomeScreen
{
    public class Plugin : Plugin<Config>
    {
        internal static Plugin singleton;

        private EventHandlers ev;

        public override void OnEnabled()
		{
            base.OnEnabled();

            singleton = this;

            ev = new EventHandlers();

            Exiled.Events.Handlers.Server.RoundStarted += ev.OnRoundStart;
            Exiled.Events.Handlers.Server.WaitingForPlayers += ev.OnWaitingForPlayers;

            Exiled.Events.Handlers.Player.Verified += ev.OnPlayerVerified;
		}

        public override void OnDisabled()
		{
            base.OnDisabled();

            Exiled.Events.Handlers.Server.RoundStarted -= ev.OnRoundStart;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= ev.OnWaitingForPlayers;

            Exiled.Events.Handlers.Player.Verified -= ev.OnPlayerVerified;

            ev = null;
        }

        public override string Author => "Cyanox";
    }
}
