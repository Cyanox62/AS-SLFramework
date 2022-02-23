using Exiled.API.Features;
using System.Collections.Generic;

namespace TipSystem.API
{
    internal class HintData
    {
        public string text;
        public float time;
    }

    public static class System
	{
        public static void ShowHint(Player player, string hint, float time)
        {
            HintData data = new HintData()
            {
                text = hint,
                time = time
            };

            Log.Warn(hint.Replace("<", "[").Replace(">", "]").Replace("\n", "/n"));

            if (!Plugin.hintQueue.ContainsKey(player.UserId))
            {
                Plugin.hintQueue.Add(player.UserId, new List<HintData>() { data });
            }
            else Plugin.hintQueue[player.UserId].Add(data);
        }
    }
}
