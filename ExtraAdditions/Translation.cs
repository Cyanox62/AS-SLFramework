using Exiled.API.Interfaces;

namespace ExtraAdditions
{
	public class Translation : ITranslation
	{
		public string BrokenElevator { get; private set; } = "This elevator is out of service. Please try again later.";
	}
}
