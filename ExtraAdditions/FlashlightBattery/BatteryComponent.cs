using Exiled.API.Features;
using Exiled.API.Features.Items;
using InventorySystem.Items;
using InventorySystem.Items.Flashlight;
using MEC;
using UnityEngine;
using Utils.Networking;

namespace ExtraAdditions.FlashlightBattery
{
	class BatteryComponent : MonoBehaviour
	{
		private float MaxBattery = Plugin.singleton.Config.FlashlightBattery;
		private float CurrentBattery;
		private bool IsDraining = false;
		private bool IsDead = false;
		private Player player;

		public void Init(Player player, float battery, bool isDead)
		{
			this.player = player;
			CurrentBattery = battery;
			IsDead = isDead;
		}

		public void Init(Player player)
		{
			this.player = player;
			CurrentBattery = MaxBattery;
			IsDead = false;
		}

		private void Update()
		{
			if (IsDraining && !IsDead)
			{
				CurrentBattery = Mathf.Clamp(CurrentBattery - Time.deltaTime, 0f, MaxBattery);
				if (CurrentBattery <= 0f)
				{
					KillFlashlight();
				}
			}
		}

		private void KillFlashlight()
		{
			if (!IsDead)
			{
				TurnOffFlashlight();
				if (EventHandlers.flashlightHints.ContainsKey(player))
				{
					Timing.KillCoroutines(EventHandlers.flashlightHints[player]);
					EventHandlers.flashlightHints.Remove(player);
				}
				IsDead = true;
			}
		}

		public void TurnOffFlashlight()
		{
			if (player.CurrentItem is Flashlight flashlight)
			{
				flashlight.Base._nextAllowedTime = Time.time + 0.6f;
				flashlight.Active = false;
				new FlashlightNetworkHandler.FlashlightMessage(flashlight.Serial, flashlight.Active).SendToAuthenticated();
			}
		}

		public float GetMaxBattery() => MaxBattery;
		public float GetRemainingBattery() => CurrentBattery;
		public bool IsBatteryDead() => IsDead;
		public void SetDraining(bool val) => IsDraining = val;
	}
}
