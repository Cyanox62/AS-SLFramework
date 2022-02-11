using HarmonyLib;
using Interactables.Interobjects;
using Interactables.Interobjects.DoorUtils;
using System.Linq;

namespace ImprovedSpectator.Patches
{
	[HarmonyPatch(typeof(BasicDoor), nameof(BasicDoor.PermissionsDenied))]
	class DoorAccess1
	{
		public static bool Prefix(ReferenceHub ply) => !EventHandlers.ghostPlayers.Select(x => x.ReferenceHub).Contains(ply);
	}

	[HarmonyPatch(typeof(CheckpointDoor), nameof(CheckpointDoor.PermissionsDenied))]
	class DoorAccess2
	{
		public static bool Prefix(ReferenceHub ply) => !EventHandlers.ghostPlayers.Select(x => x.ReferenceHub).Contains(ply);
	}
}
