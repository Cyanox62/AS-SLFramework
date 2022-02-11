using Exiled.API.Features;

namespace PeanutExplodes
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

			Exiled.Events.Handlers.Player.Dying += ev.OnPlayerDeath;
		}

		public override void OnDisabled()
		{
			base.OnDisabled();

			Exiled.Events.Handlers.Player.Dying -= ev.OnPlayerDeath;

			ev = null;
		}

		public override string Author => "Cyanox";
		public override string Name => "PeanutExplodes";
	}
}
