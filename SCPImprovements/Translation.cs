using Exiled.API.Interfaces;

namespace SCPImprovements
{
	public class Translation : ITranslation
	{
		public string ReplaceNotice { get; private set; } = "At this time, SCP-079 is not allowed to play as the only SCP. You have been respawned as {newSCP}";
	}
}
