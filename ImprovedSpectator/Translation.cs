using Exiled.API.Interfaces;

namespace ImprovedSpectator
{
	public class Translation : ITranslation
	{
		public string MTF { get; private set; } = "<color=blue>Nine-Tailed Fox</color>";
		public string CI { get; private set; } = "<color=green>Chaos Insurgency</color>";
		public string RespawnTime { get; private set; } = "Next respawn in ";
		public string RespawnTeam { get; private set; } = "Team to respawn: ";
		public string Minutes { get; private set; } = "<color=orange><b>{minutes}:</b></color>";
		public string Seconds { get; private set; } = "<color=orange><b>{seconds}</b></color>";
	}
}
