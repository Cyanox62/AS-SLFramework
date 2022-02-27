﻿using Exiled.API.Interfaces;

namespace ExtraAdditions
{
	public class Translation : ITranslation
	{
		// Evelator Failure

		public string BrokenElevator { get; private set; } = "This elevator is out of service. Please try again later.";

		// Flashlight Battery

		public string FlashlightBattery { get; private set; } = "Flashlight Battery: {percent}%";
	}
}
