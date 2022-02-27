using Exiled.API.Features;
using UnityEngine;

namespace ExtraAdditions.FlashlightBattery
{
	class BatteryComponent : MonoBehaviour
	{
		// 4 min
		private const float MaxBattery = 240f;
		private float CurrentBattery;

		public void Init(float battery = MaxBattery)
		{
			CurrentBattery = battery;
		}

		private void Update()
		{
			CurrentBattery = Mathf.Clamp(CurrentBattery - Time.deltaTime, 0, MaxBattery);
		}

		public float GetMaxBattery() => MaxBattery;
		public float GetRemainingBattery() => CurrentBattery;
	}
}
