using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs;
using Interactables.Interobjects.DoorUtils;
using Newtonsoft.Json;
using System.IO;
using System.Linq;

namespace ExtraUtilities
{
	class EventHandlers
	{
		// Thanks Beryl <3
		// https://github.com/SebasCapo/RemoteKeycard
		internal void OnDoorAccess(InteractingDoorEventArgs ev)
		{
			if (Plugin.singleton.Config.RequireHeldKeycard) return;

			if (!ev.IsAllowed && ev.Player.Items.Any(item => item is Keycard keycard && (keycard.Base.Permissions & ev.Door.RequiredPermissions.RequiredPermissions) != 0))
			{
				ev.IsAllowed = true;
			}
		}

		internal void OnLockerAccess(InteractingLockerEventArgs ev)
		{
			if (Plugin.singleton.Config.RequireHeldKeycard) return;

			if (!ev.IsAllowed && ev.Chamber != null && ev.Player.Items.Any(item => item is Keycard keycard && keycard.Base.Permissions.HasFlagFast(ev.Chamber.RequiredPermissions)))
			{
				ev.IsAllowed = true;
			}
		}

		internal void OnGeneratorUnlock(UnlockingGeneratorEventArgs ev)
		{
			if (Plugin.singleton.Config.RequireHeldKeycard) return;

			if (!ev.IsAllowed && ev.Player.Items.Any(item => item is Keycard keycard && (keycard.Base.Permissions & ev.Generator.Base._requiredPermission) != 0))
			{
				ev.IsAllowed = true;
			}
		}
	}
}
