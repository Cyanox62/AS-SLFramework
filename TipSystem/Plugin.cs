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
						List<string> finalHint = new List<string>();
						for (int i = hintQueue[player.UserId].Count - 1; i >= 0 ; i--)
						{
							HintData hint = hintQueue[player.UserId][i];
							if (hint.time > 0f)
							{
								char[] chars = hint.text.ToCharArray();
								List<string> finishedHint = new List<string>();
								bool lastFoundWords = false;
								int wordsOffset = 0;
								for (int j = 0; j < chars.Length; j++)
								{
									char cur = chars[j];

									if (lastFoundWords && cur != '\n')
									{
										wordsOffset++;
										continue;
									}
									else lastFoundWords = false;

									string t = string.Empty;
									int indx = j;
									while (chars.Length != indx && chars[indx] != '\n')
									{
										lastFoundWords = true;
										t += chars[indx];
										indx++;
									}

									string append = t != string.Empty ? t : "\n";

									if (finishedHint.Count == j - wordsOffset)
									{
										finishedHint.Add(append);
									}
									else
									{
										finishedHint[j - wordsOffset] = append;
									}
								}

								List<string> temp = new List<string>(finalHint);
								for (int j = 0; j < finishedHint.Count; j++)
								{
									string fin = finishedHint[j];
									if (fin != "\n")
									{
										if (temp.Count == j) temp.Add(fin);
										else temp[j] = fin;
									}
									else if (temp.Count == j)
									{
										temp.Add("\n");
									}
								}
								finalHint = new List<string>(temp);
								hint.time -= hintInterval;
							}
							else
							{
								hintQueue[player.UserId].Remove(hint);
							}
						}

						string a = string.Empty;
						for (int i = 0; i < finalHint.Count; i++)
						{
							a += finalHint[i];
						}
						player.ShowHint(a, hintInterval * 2f);
					}
                }      
            }
        }

        public override string Author => "Cyanox";
        public override string Name => "TipSystem";
    }
}
