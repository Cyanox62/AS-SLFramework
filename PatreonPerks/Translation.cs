using Exiled.API.Interfaces;

namespace PatreonPerks
{
	public class Translation : ITranslation
	{
		public string AnnounceJoin { get; private set; } = "{player} has joined the server!";
	}
}
