using Exiled.API.Interfaces;

namespace ExtraAdditions
{
	public class Translation : ITranslation
	{
		// Remote Keycard

		public string AccessDenied { get; private set; } = "You do not have the correct clearance. You require {permission} access.";
		public string FacilityManager { get; private set; } = "Facility Manager";
		public string Guard { get; private set; } = "Guard";
		public string MTFPrivate { get; private set; } = "MTF Private";
		public string ResearchSupervisor { get; private set; } = "Research Supervisor";
		public string Janitor { get; private set; } = "Janitor";
		public string Scientist { get; private set; } = "Scientist";
		public string ContainmentEngineer { get; private set; } = "Containment Engineer";
		public string MTFSergeant { get; private set; } = "MTF Sergeant";
		public string MTFCaptain { get; private set; } = "MTF Captain";

		// Evelator Failure

		public string BrokenElevator { get; private set; } = "This elevator is out of service. Please try again later.";

		// Flashlight Battery

		public string FlashlightBattery { get; private set; } = "Flashlight Battery: {percent}%";
		public string FlashlightIsDead { get; private set; } = "The flashlight is out of battery.";

		// Misc

		public string WarheadDetonation { get; private set; } = "<color=red>Warhead Detonation in {seconds} seconds</color>";
		public string Decontamination { get; private set; } = "<color=red>Decontamination in {seconds} seconds</color>";
		public string CassieInUse { get; private set; } = "CASSIE IN PROGRESS. PLEASE WAIT.";
		public string UnitMessage { get; private set; } = "discord.gg/yourlink";
	}
}
