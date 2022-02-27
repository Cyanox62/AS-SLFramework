using Exiled.API.Features;
using UnityEngine;

namespace ExtraAdditions.FlashlightBattery
{
	class BatteryComponent : MonoBehaviour
	{
		private float MaxBattery = Plugin.singleton.Config.FlashlightBattery;
		private float CurrentBattery;
		private bool IsDraining = false;

		public void Init(float battery)
		{
			CurrentBattery = battery;
		}

		private void Update()
		{
			if (IsDraining) CurrentBattery = Mathf.Clamp(CurrentBattery - Time.deltaTime, 0, MaxBattery);
		}

		public float GetMaxBattery() => MaxBattery;
		public float GetRemainingBattery() => CurrentBattery;
		public void SetDraining(bool val) => IsDraining = val;
	}
}
