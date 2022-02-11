using Exiled.API.Features;

namespace ExtraUtilities
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
			Exiled.Events.Handlers.Player.InteractingDoor += ev.OnDoorAccess;
			Exiled.Events.Handlers.Player.InteractingLocker += ev.OnLockerAccess;
			Exiled.Events.Handlers.Player.UnlockingGenerator += ev.OnGeneratorUnlock;
		}

		public override void OnDisabled()
		{
			base.OnDisabled();

			Exiled.Events.Handlers.Player.InteractingDoor -= ev.OnDoorAccess;
			Exiled.Events.Handlers.Player.InteractingLocker -= ev.OnLockerAccess;
			Exiled.Events.Handlers.Player.UnlockingGenerator -= ev.OnGeneratorUnlock;
			ev = null;
		}

		public override string Author => "Cyanox";
		public override string Name => "ExtraUtilities";
	}
}
