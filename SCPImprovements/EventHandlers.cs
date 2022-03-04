using Exiled.API.Features;
using Exiled.API.Features.Roles;
using Exiled.Events.EventArgs;
using MEC;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SCPImprovements
{
	class EventHandlers
	{
		private Dictionary<Player, CoroutineHandle> scp096coroutines = new Dictionary<Player, CoroutineHandle>();

		private bool is106Contained, canChange;
		internal static bool isLastAlive;
		private const int WorldMask = 1207976449;

		private List<RoleType> scp079Respawns = new List<RoleType>()
		{
			RoleType.Scp049,
			RoleType.Scp096,
			RoleType.Scp106,
			RoleType.Scp93953,
			RoleType.Scp93989
		};

		internal void OnPlayerDeath(DyingEventArgs ev)
		{
			if (ev.Target.Role.Team == Team.SCP)
			{
				Timing.RunCoroutine(Check079());
			}
		}

		public void OnDetonated() => canChange = false;

		private IEnumerator<float> Check079()
		{
			if (canChange)
			{
				yield return Timing.WaitForSeconds(3f);
				IEnumerable<Player> enumerable = Player.Get(Team.SCP).Where(x => x.Role != RoleType.Scp0492);
				List<Player> pList = enumerable.ToList();
				if (pList.Count == 1 && pList[0].Role == RoleType.Scp079)
				{
					isLastAlive = true;
					Player player = pList[0];
					int level = player.Role.As<Scp079Role>().Level;
					RoleType role = scp079Respawns[UnityEngine.Random.Range(0, scp079Respawns.Count)];
					if (is106Contained && role == RoleType.Scp106) role = RoleType.Scp93953;
					player.SetRole(role);
					Plugin.AccessHintSystem(player, new string('\n', Plugin.singleton.Config.Scp079ReplaceTextLower) + Plugin.singleton.Translation.ReplaceNotice.Replace("{newSCP}", role.ToString().ToUpper()), Plugin.singleton.Config.ComputerSwapHintTime);
				}
			}
		}

		internal void OnRoundStart()
		{
			is106Contained = false;
			canChange = true;
			isLastAlive = false;
		}

		public void OnScp106Contain(ContainingEventArgs ev)
		{
			is106Contained = true;
		}

		public void OnCassie(SendingCassieMessageEventArgs ev)
		{
			if (ev.Words.Contains("allgeneratorsengaged") && isLastAlive) ev.IsAllowed = false;
		}

		internal void OnEndRage(CalmingDownEventArgs ev)
		{
			foreach (ReferenceHub p in ev.Scp096._targets)
			{
				Player player = Player.Get(p);
				Log.Warn(player.Nickname);
				if (player != null) Plugin.AccessHintSystem(player, Plugin.singleton.Translation.NoLongerSeeing096, Plugin.singleton.Config.Scp096ReliefTime);
			}
		}

		internal void OnAddTarget(AddingTargetEventArgs ev)
		{
			Plugin.AccessHintSystem(ev.Target, Plugin.singleton.Translation.TargetOf096, Plugin.singleton.Config.Scp096TargetHintTime);
		}

		internal void OnChangeRole(ChangingRoleEventArgs ev)
		{
			if (scp096coroutines.ContainsKey(ev.Player))
			{
				Timing.KillCoroutines(scp096coroutines[ev.Player]);
				scp096coroutines.Remove(ev.Player);
			}
			if (ev.NewRole == RoleType.Scp096)
			{
				Timing.CallDelayed(1f, () => scp096coroutines.Add(ev.Player, Timing.RunCoroutine(LookingAtScp096(ev.Player))));
			}
		}

		private bool IsLookingAt(Player player, Player target)
		{
			Vector3 targetPos = target.Position;
			return Vector3.Angle((targetPos - player.Position).normalized, player.CameraTransform.forward) <= 45f && !Physics.Linecast(player.Position, target.Position, WorldMask);
		}

		private IEnumerator<float> LookingAtScp096(Player scp)
		{
			if (scp.Role.Is(out Scp096Role scp096))
			{
				while (Round.IsStarted)
				{
					yield return Timing.WaitForSeconds(0.5f);
					foreach (Player player in Player.Get(x => x.IsHuman))
					{
						if (!scp096.Targets.Contains(player) && IsLookingAt(player, scp))
						{
							Plugin.AccessHintSystem(player, $"{new string('\n', Plugin.singleton.Config.Scp096VisionTextLower)}{Plugin.singleton.Translation.CanSee096}", 1f);
						}
					}
				}
			}
		}
	}
}
