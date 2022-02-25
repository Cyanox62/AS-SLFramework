using Newtonsoft.Json;

namespace PatreonPerks.Perks
{
	public interface IPerk
	{
		string PerkName { get; }

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		string Param { get; set; }
	}
}