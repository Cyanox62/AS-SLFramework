using Exiled.API.Interfaces;

namespace ServerStatistics
{
	public class Translation : ITranslation
	{
		// Command Log
		public string CommandMessage { get; private set; } = ":keyboard: [RA] {commandSender} ({commandUserid}) >: {command}";

		// Ban Log
		public string KickMessage { get; private set; } = ":hammer: Player {targetNickname} ({targetUserid}) was kicked by {senderNickname} ({senderUserid}).";
		public string BanMessage { get; private set; } = ":hammer: Player {targetNickname} ({targetUserid}) was banned ({time}) by {senderNickname} ({senderUserid}).";
		public string MuteMessage { get; private set; } = ":hammer: Player {targetNickname} ({targetUserid}) was muted by {senderNickname} ({senderUserid}).";

		// Event Log
		public string RoundStart { get; private set; } = ":arrow_forward: A new round has started with {playerCount} players!";
		public string RoundEnd { get; private set; } = ":arrow_forward: The round has ended!";
		public string RoundRestart { get; private set; } = ":arrow_forward: Round restarting...";
		public string WaitingForPlayers { get; private set; } = ":arrow_forward: Server is ready for players!";
		public string NukeStart { get; private set; } = ":arrow_forward: The warhead has started!";
		public string NukeStop { get; private set; } = ":arrow_forward: The warhead has been cancelled!";
		public string NukeDetonate { get; private set; } = ":arrow_forward: The warhead has detonated!";
		public string Decontamination { get; private set; } = ":arrow_forward: Light Containment Zone has begun decontamination!";
		public string TeamRespawn { get; private set; } = ":arrow_forward: {team} has respawned!";
	}
}
