using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Exiled.Loader;
using MEC;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace WelcomeScreen
{
	class EventHandlers
	{
		private CoroutineHandle coroutine;

		internal void OnWaitingForPlayers()
		{
			if (Plugin.singleton.Config.ShowHint) coroutine = Timing.RunCoroutine(HintCoroutine());
		}

		internal void OnRoundStart() => Timing.KillCoroutines(coroutine);

		internal void OnPlayerVerified(VerifiedEventArgs ev)
		{
			ev.Player.Broadcast((ushort)Plugin.singleton.Config.BroadcastTime, Plugin.singleton.Translation.WelcomeMessage);
		}

		private int CallTokenAPI(Player player, string method)
		{
			var plugin = Loader.Plugins.First(pl => pl.Name == "TokenShop");
			var asm = plugin?.Assembly;
			var type = asm?.GetType("TokenShop.API.Data");
			var m = type?.GetMethod(method, BindingFlags.Public | BindingFlags.Static);
			if (plugin != null && asm != null && type != null && m != null)
			{
				return (int)m.Invoke(null, new object[] { player.UserId });
			}
			else return -1;
		}

		private IEnumerator<float> HintCoroutine()
		{
			while (!Round.IsStarted)
			{
				yield return Timing.WaitForSeconds(1f);

				foreach (Player player in Player.List)
				{
					int tokens = CallTokenAPI(player, "GetTokens");
					float hours = CallTokenAPI(player, "GetPlaytime") / 60f;
					Plugin.AccessHintSystem(player, $"{Plugin.singleton.Translation.ServerNumberText.Insert(0, new string('\n', Plugin.singleton.Config.TextLower)).Replace("{serverNum}", Plugin.singleton.Config.ServerNumber.ToString())}\n{Plugin.singleton.Translation.DiscordLink}" +
						$"{(Plugin.singleton.Translation.TokenData != string.Empty ? $"\n{Plugin.singleton.Translation.TokenData.Replace("{tokens}", tokens.ToString()).Replace("{playtime}", $"{hours.ToString("0.0")} hour{(hours != 1 ? "s" : string.Empty)}")}" : string.Empty)}" +
						$"{(Plugin.singleton.Translation.Warning != string.Empty ? $"\n{Plugin.singleton.Translation.Warning}" : string.Empty)}", 2f);
				}
			}
		}
	}
}
