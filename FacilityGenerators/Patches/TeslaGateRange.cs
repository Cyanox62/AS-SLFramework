using HarmonyLib;

namespace FacilityGenerators.Patches
{
	[HarmonyPatch(typeof(TeslaGate), nameof(TeslaGate.PlayerInIdleRange)]
	class TeslaGateRange
	{
		public static void Postfix(ref bool __result) => __result = false;
	}
}