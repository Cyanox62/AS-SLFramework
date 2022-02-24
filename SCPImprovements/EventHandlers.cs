using Exiled.API.Features;
using Exiled.API.Features.Roles;
using Exiled.Events.EventArgs;
using MEC;
using System.Collections.Generic;
using System.Linq;

namespace SCPImprovements
{
	class EventHandlers
	{
		internal static List<Player> isLookingAt096 = new List<Player>();
		internal static Dictionary<Player, CoroutineHandle> lookingAt096Cooldown = new Dictionary<Player, CoroutineHandle>();

		private bool is106Contained, canChange;
		internal static bool isLastAlive;

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

		internal static IEnumerator<float> StartSafeDelay(Player p)
		{
			yield return Timing.WaitForSeconds(Plugin.singleton.Config.Scp096ReliefOffset);
			if (lookingAt096Cooldown.ContainsKey(p))
			{
				Plugin.AccessHintSystem(p, Plugin.singleton.Translation.NoLongerSeeing096, Plugin.singleton.Config.Scp096ReliefTime);
				lookingAt096Cooldown.Remove(p);
			}
		}

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
					Plugin.AccessHintSystem(player, new string('\n', Plugin.singleton.Config.TextLower) + Plugin.singleton.Translation.ReplaceNotice.Replace("{newSCP}", role.ToString().ToUpper()), Plugin.singleton.Config.ComputerSwapHintTime);
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
	}
}
