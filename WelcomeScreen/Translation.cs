using Exiled.API.Interfaces;

namespace WelcomeScreen
{
	public class Translation : ITranslation
	{
		public string WelcomeMessage { get; private set; } = "Welcome to Astrios SCP:SL!";
		public string ServerNumberText { get; private set; } = "Server {serverNum}";
		public string DiscordLink { get; private set; } = "discord.gg/yourlink";
		public string TokenData { get; private set; } = "Tokens: {tokens}\nPlaytime: {playtime}";
	}
}
