using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Exiled.API.Extensions;
using TokenShop.Commands;
using TokenShop.Perks;

namespace TokenShop
{
	class EventHandlers
	{
		internal static Dictionary<string, CoroutineHandle> playerCoroutines = new Dictionary<string, CoroutineHandle>();
		internal static Dictionary<string, TokenStats> playerStats = new Dictionary<string, TokenStats>();

		private Dictionary<Player, bool> survivingPlayers = new Dictionary<Player, bool>();

		internal void OnPlayerVerified(VerifiedEventArgs ev)
		{
			try
			{
				if (!playerStats.ContainsKey(ev.Player.UserId))
				{
					string path = $"{Path.Combine(Plugin.FolderFilePath, ev.Player.UserId)}.json";
					if (!File.Exists(path)) File.WriteAllText(path, JsonConvert.SerializeObject(new TokenStats(), Formatting.Indented));
					TokenStats data = JsonConvert.DeserializeObject<TokenStats>(File.ReadAllText(path));
					data.path = path;
					playerStats.Add(ev.Player.UserId, data);

					// Validate perks
					for (int i = playerStats[ev.Player.UserId].perks.Count - 1; i >= 0; i--)
					{
						var entry = playerStats[ev.Player.UserId].perks[i];
						ShopItem item = Shop.ShopItems.FirstOrDefault(x => i == x.id);
						if (item == null || item.perk != entry)
						{
							playerStats[ev.Player.UserId].perks.Remove(i);
							Log($"Detected invalid perk at id \"{i}\" for {ev.Player.UserId}, removing..");
						}
					}

					playerCoroutines.Add(ev.Player.UserId, Timing.RunCoroutine(PlaytimeCoroutine(ev.Player)));
					Log($"Loaded stats for {ev.Player.UserId}");
				}
			} 
			catch (Exception x)
			{
				Exiled.API.Features.Log.Error($"Failed to load stats for {ev.Player.UserId}: {x.Message}");
			}
		}

		/*internal void OnPlayerDeath(DyingEventArgs ev)
		{
			if (playerStats.ContainsKey(ev.Target.UserId))
			{
				CustomDeathReason perk = (CustomDeathReason)playerStats[ev.Target.UserId].perks.Values.FirstOrDefault(x => x is CustomDeathReason);
				if (perk != null)
				{
					perk.
				}
			}
		}*/

		internal void OnPickingUpItem(PickingUpItemEventArgs ev)
		{
			if (ev.Pickup.Type == ItemType.Coin)
			{
				GiveTokens(ev.Player, Plugin.singleton.Config.CoinPickupTokens, Plugin.singleton.Translation.CoinReason);
				ev.IsAllowed = false;
				ev.Pickup.Destroy();
			}
		}

		internal void OnSetRole(ChangingRoleEventArgs ev)
		{
			if (!survivingPlayers.ContainsKey(ev.Player))
			{
				if (ev.NewRole == RoleType.ClassD ||
				ev.NewRole == RoleType.Scientist ||
				ev.NewRole == RoleType.FacilityGuard ||
				ev.NewRole.GetTeam() == Team.SCP)
				{
					survivingPlayers.Add(ev.Player, true);
					Log($"Player {ev.Player.UserId} was added as a surviving player.");
				}
			}
			else if (survivingPlayers[ev.Player])
			{
				survivingPlayers[ev.Player] = false;
				Log($"Player {ev.Player.UserId} was removed as a surviving player.");
			}
		}

		internal void OnRoundStart()
		{
			// grant perks
			Timing.CallDelayed(3f, () => 
			{
				Log("Attempting to grant perks..");
				foreach (Player player in Player.List)
				{
					if (playerStats.ContainsKey(player.UserId))
					{
						foreach (int i in playerStats[player.UserId].perks.Keys)
						{
							ShopItem shopItem = Shop.ShopItems[i];
							if (shopItem.perk is ParamaterizedItem pPerk)
							{
								if (Enum.TryParse(pPerk.Param.ToString(), out ItemType item))
								{
									// Param is spawn item
									player.AddItem(item);
								}

								// Add support for more types of params

								if (!pPerk.IsPermanent)
								{
									playerStats[player.UserId].perks.Remove(i);
								}
								Log($"Granted user {player.UserId} param item {pPerk.Param}");
							}
							/*else if (shopItem.perk is CustomDeathReason cPerk)
							{
								// custom death reason
								Log($"Granted user {player.UserId} custom death reason");
							}*/
						}
					}
				}
				Log("Finished granting perks!");
			});
		}

		internal void OnRoundEnd(RoundEndedEventArgs ev)
		{
			// Check if survived the entire round
			foreach (Player player in survivingPlayers.Keys)
			{
				if (player.Role.Team == Team.SCP)
				{
					GiveTokens(player, Plugin.singleton.Config.ScpSurviveTokens, Plugin.singleton.Translation.ScpSurviveReason);
				}
				else
				{
					GiveTokens(player, Plugin.singleton.Config.HumanSurviveTokens, Plugin.singleton.Translation.HumanSurviveReason);
				}
			}

			// Save stats
			foreach (var entry in playerStats)
			{
				try
				{
					entry.Value.playtime += RoundSummary.roundTime;
					File.WriteAllText(entry.Value.path, JsonConvert.SerializeObject(entry.Value, Formatting.Indented));
					Log($"Successfully saved data for {entry.Key}");
				}
				catch (Exception x)
				{
					Exiled.API.Features.Log.Error($"Failed to save stats for {entry.Key}: {x.Message}");
				}
			}

			// Clear data
			playerStats.Clear();
			Timing.KillCoroutines(playerCoroutines.Values.ToArray());
			playerCoroutines.Clear();

			survivingPlayers.Clear();
		}

		internal void OnPlayerLeft(LeftEventArgs ev)
		{
			if (playerCoroutines.ContainsKey(ev.Player.UserId))
			{
				Timing.KillCoroutines(playerCoroutines[ev.Player.UserId]);
				playerCoroutines.Remove(ev.Player.UserId);
			}
		}

		// Methods

		internal static void GiveTokens(Player player, int tokens, string reason, bool alert = true)
		{
			if (playerStats.ContainsKey(player.UserId))
			{
				if (Plugin.singleton.Config.RoleTokenMultipliers.ContainsKey(player.GroupName))
				{
					tokens = (int)(tokens * Plugin.singleton.Config.RoleTokenMultipliers[player.GroupName]);
				}
				playerStats[player.UserId].tokens += tokens;
				if (alert) Plugin.AccessHintSystem(player, new string('\n', Plugin.singleton.Config.TextLower) + Plugin.singleton.Translation.TokensEarned
					.Replace("{tokens}", tokens.ToString())
					.Replace("{reason}", reason), Plugin.singleton.Config.TokenHintTime);
				Log($"Granted {player.UserId} {tokens} tokens for reason: {reason}");
			}
		}

		internal static void Log(string msg)
		{
			if (Plugin.singleton.Config.IsDebug) Exiled.API.Features.Log.Info(msg);
		}

		// Coroutines

		private IEnumerator<float> PlaytimeCoroutine(Player player)
		{
			while (player != null)
			{
				yield return Timing.WaitForSeconds(Plugin.singleton.Config.PlaytimeTokenInterval);
				if (Round.IsStarted)
				{
					if (!Plugin.singleton.Config.GiveOverwatchTokens && player.IsOverwatchEnabled) continue;
					GiveTokens(player, Plugin.singleton.Config.PlaytimeTokenAmount, Plugin.singleton.Translation.PlaytimeReason);
				}
			}
		}
	}
}
