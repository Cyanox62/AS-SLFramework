using Exiled.API.Interfaces;

namespace TokenShop
{
	public class Translation : ITranslation
	{
		public string TokensEarned { get; private set; } = "You've received <color=yellow>{tokens} tokens</color> for {reason}!";
		public string PlaytimeReason { get; private set; } = "playing on the server";
		public string CoinReason { get; private set; } = "picking up a coin";
		public string HumanSurviveReason { get; private set; } = "surviving as a human";
		public string ScpSurviveReason { get; private set; } = "surviving as an SCP";
	}
}
