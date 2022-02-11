using Exiled.API.Interfaces;

namespace ImprovedSpectator
{
	public class Translation : ITranslation
	{
		public string MTF { get; private set; } = "<color=blue>Nine-Tailed Fox</color>";
		public string CI { get; private set; } = "<color=green>Chaos Insurgency</color>";
		public string RespawnTime { get; private set; } = "Next respawn in {minutes}:{seconds}";
		public string RespawnInProgress { get; private set; } = "{team} respawn in progress";
		public string RespawnTeam { get; private set; } = "Team to respawn: {team}";
	}
}
