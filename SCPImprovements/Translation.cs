using Exiled.API.Interfaces;

namespace SCPImprovements
{
	public class Translation : ITranslation
	{
		public string ReplaceNotice { get; private set; } = "At this time, SCP-079 is not allowed to play as the only SCP. You have been respawned as {newSCP}";
		public string CanSee096 { get; private set; } = "Careful! SCP-096 is within your sight line.";
		public string TargetOf096 { get; private set; } = "SCP-096 has spotted and is hunting you. RUN!";
		public string NoLongerSeeing096 { get; private set; } = "A wave of relief rushes over you as SCP-096 is no longer hunting you.";
	}
}
