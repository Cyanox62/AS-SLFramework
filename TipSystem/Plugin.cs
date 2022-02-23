using Exiled.API.Features;
using MEC;
using System.Collections.Generic;
using System.Linq;
using TipSystem.API;

namespace TipSystem
{
    public class Plugin : Plugin<Config>
    {
        internal static Dictionary<string, List<HintData>> hintQueue = new Dictionary<string, List<HintData>>();

        private CoroutineHandle coroutine;

        public override void OnEnabled()
        {
            base.OnEnabled();

            coroutine = Timing.RunCoroutine(ShowHint());
        }

        public override void OnDisabled()
        {
            base.OnDisabled();

            Timing.KillCoroutines(coroutine);
        }

        // Coroutines

        const float hintInterval = 0.5f;
        
        private IEnumerator<float> ShowHint()
		{
            while (true)
			{
                yield return Timing.WaitForSeconds(hintInterval);
                foreach (Player player in Player.List)
				{
                    if (hintQueue.ContainsKey(player.UserId))
                    {
                        string finalHint = string.Empty;
                        int curNewlines = 0;
                        foreach (HintData hint in hintQueue[player.UserId].OrderBy(x => x.text.Count(c => c == '\n')))
                        {
                            if (hint.time > 0f)
                            {
                                string curHint = hint.text;
                                curHint = curHint.Substring(curNewlines);
                                curNewlines += curHint.Count(c => c == '\n');
                                finalHint += curHint;
                                hint.time -= hintInterval;
                            }
                            else
                            {
                                hintQueue[player.UserId].Remove(hint);
                            }
                        }
                        player.ShowHint(finalHint, hintInterval * 2f);
                    }
                }      
            }
        }

        public override string Author => "Cyanox";
        public override string Name => "TipSystem";
    }
}
