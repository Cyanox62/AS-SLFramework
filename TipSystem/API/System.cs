using Exiled.API.Features;
using System.Collections.Generic;
using System.Linq;

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

            //Log.Warn(hint.Replace("<", "[").Replace(">", "]").Replace("\n", "/n"));

            if (!Plugin.hintQueue.ContainsKey(player.UserId))
            {
                Plugin.hintQueue.Add(player.UserId, new List<HintData>() { data });
            }
            else 
            {
                foreach (HintData h in Plugin.hintQueue[player.UserId])
                {
                    if (h.text == data.text)
                    {
                        h.time = data.time;
                        return;
                    }
                }
                Plugin.hintQueue[player.UserId].Add(data);
            }
        }

        public static void ClearHints(Player player, string filter = "")
        {
            if (Plugin.hintQueue.ContainsKey(player.UserId))
            {
                if (filter != string.Empty)
				{
                    for (int i = Plugin.hintQueue.Count - 1; i >= 0; i--)
					{
                        var entry = Plugin.hintQueue.ElementAt(i);
                        for (int a = entry.Value.Count - 1; a >= 0; a--)
						{
                            HintData hint = entry.Value[a];
                            if (hint.text.Contains(filter))
                            {
                                Plugin.hintQueue[entry.Key].Remove(hint);
                            }
                        }
					}
				}
                else
				{
                    Plugin.hintQueue.Remove(player.UserId);
                    player.ShowHint(string.Empty, 0.1f);
                }
            }
        }
    }
}
