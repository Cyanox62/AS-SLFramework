using Exiled.API.Features.Items;
using Exiled.Events.EventArgs;

namespace PeanutExplodes
{
	class EventHandlers
	{
		public void OnPlayerDeath(DyingEventArgs ev)
		{
			if (ev.Target.Role == RoleType.Scp173)
			{
				for (int i = 0; i < Plugin.singleton.Config.Magnitude; i++)
				{
					new ExplosiveGrenade(ItemType.GrenadeHE, ev.Target) { FuseTime = 0.15f }.SpawnActive(ev.Target.Position, ev.Target);
				}
			}
		}
	}
}
