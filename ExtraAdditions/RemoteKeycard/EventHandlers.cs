using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs;
using Interactables.Interobjects.DoorUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtraAdditions.RemoteKeycard
{
	class EventHandlers
	{
		private Dictionary<KeycardPermissions, string> keycardPerms = new Dictionary<KeycardPermissions, string>()
		{
			{ KeycardPermissions.AlphaWarhead, Plugin.singleton.Translation.FacilityManager },
			{ KeycardPermissions.ArmoryLevelOne, Plugin.singleton.Translation.Guard },
			{ KeycardPermissions.ArmoryLevelTwo, Plugin.singleton.Translation.MTFPrivate },
			{ KeycardPermissions.ArmoryLevelThree, Plugin.singleton.Translation.MTFCaptain },
			{ KeycardPermissions.Checkpoints, Plugin.singleton.Translation.ResearchSupervisor },
			{ KeycardPermissions.ContainmentLevelOne, Plugin.singleton.Translation.Janitor },
			{ KeycardPermissions.ContainmentLevelTwo, Plugin.singleton.Translation.Scientist },
			{ KeycardPermissions.ContainmentLevelThree, Plugin.singleton.Translation.ContainmentEngineer },
			{ KeycardPermissions.ExitGates, Plugin.singleton.Translation.MTFSergeant },
			{ KeycardPermissions.Intercom, Plugin.singleton.Translation.MTFCaptain }
		};

		// Thanks Beryl <3
		// https://github.com/SebasCapo/RemoteKeycard
		internal void OnDoorAccess(InteractingDoorEventArgs ev)
		{
			if (Plugin.singleton.Config.RequireHeldKeycard) return;

			if (ev.Player.Items.Any(item => item is Keycard keycard && (keycard.Base.Permissions & ev.Door.RequiredPermissions.RequiredPermissions) != 0))
			{
				if (!ev.IsAllowed) ev.IsAllowed = true;
			}
			else
			{
				int perms = (int)ev.Door.RequiredPermissions.RequiredPermissions;
				if (perms > (int)KeycardPermissions.ScpOverride) perms -= (int)KeycardPermissions.ScpOverride;
				else if (perms % 2 == 1) perms %= 2;
				if (keycardPerms.ContainsKey((KeycardPermissions)perms))
				{
					Plugin.ClearHints(ev.Player, Plugin.singleton.Translation.AccessDenied.Substring(0, Plugin.singleton.Translation.AccessDenied.IndexOf("{") - 2));
					Plugin.AccessHintSystem(ev.Player, $"{new string('\n', Plugin.singleton.Config.KeycardHintTextLower)}{Plugin.singleton.Translation.AccessDenied.Replace("{permission}", keycardPerms[(KeycardPermissions)perms])}", Plugin.singleton.Config.AccessDeniedHintTime);
				}
			}
		}

		internal void OnLockerAccess(InteractingLockerEventArgs ev)
		{
			if (Plugin.singleton.Config.RequireHeldKeycard) return;

			if (ev.Chamber != null && ev.Player.Items.Any(item => item is Keycard keycard && keycard.Base.Permissions.HasFlagFast(ev.Chamber.RequiredPermissions)))
			{
				if (!ev.IsAllowed) ev.IsAllowed = true;
			} 
			else
			{
				int perms = (int)ev.Chamber.RequiredPermissions;
				if (perms % 2 == 1) perms %= 2;
				if (keycardPerms.ContainsKey((KeycardPermissions)perms))
				{
					Plugin.ClearHints(ev.Player, Plugin.singleton.Translation.AccessDenied.Substring(0, Plugin.singleton.Translation.AccessDenied.IndexOf("{") - 2));
					Plugin.AccessHintSystem(ev.Player, $"{new string('\n', Plugin.singleton.Config.KeycardHintTextLower)}{Plugin.singleton.Translation.AccessDenied.Replace("{permission}", keycardPerms[(KeycardPermissions)perms])}", Plugin.singleton.Config.AccessDeniedHintTime);
				}
			}
		}

		internal void OnGeneratorUnlock(UnlockingGeneratorEventArgs ev)
		{
			if (Plugin.singleton.Config.RequireHeldKeycard) return;

			if (ev.Player.Items.Any(item => item is Keycard keycard && (keycard.Base.Permissions & ev.Generator.Base._requiredPermission) != 0))
			{
				if (!ev.IsAllowed) ev.IsAllowed = true;
			}
			else if (keycardPerms.ContainsKey(ev.Generator.Base._requiredPermission))
			{
				Plugin.ClearHints(ev.Player, Plugin.singleton.Translation.AccessDenied.Substring(0, Plugin.singleton.Translation.AccessDenied.IndexOf("{") - 2));
				Plugin.AccessHintSystem(ev.Player, $"{new string('\n', Plugin.singleton.Config.KeycardHintTextLower)}{Plugin.singleton.Translation.AccessDenied.Replace("{permission}", keycardPerms[ev.Generator.Base._requiredPermission])}", Plugin.singleton.Config.AccessDeniedHintTime);
			}
		}
	}
}
