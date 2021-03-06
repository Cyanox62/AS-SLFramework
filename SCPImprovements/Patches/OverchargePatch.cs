using HarmonyLib;

namespace SCPImprovements.Patches
{
	[HarmonyPatch(typeof(Recontainer079), nameof(Recontainer079.BeginOvercharge))]
	class OverchargePatch1
	{
		public static bool Prefix() => !EventHandlers.isLastAlive;
	}

	[HarmonyPatch(typeof(Recontainer079), nameof(Recontainer079.Recontain))]
	class OverchargePatch2
	{
		public static bool Prefix() => !EventHandlers.isLastAlive;
	}
}
