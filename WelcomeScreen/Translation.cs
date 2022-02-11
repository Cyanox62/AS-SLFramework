﻿using Exiled.API.Interfaces;

namespace WelcomeScreen
{
	public class Translation : ITranslation
	{
		public string ServerNumberText { get; private set; } = "Server {serverNum}";
		public string DiscordLink { get; private set; } = "discord.gg/yourlink";
	}
}
