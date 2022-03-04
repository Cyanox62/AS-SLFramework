using System.Collections.Generic;
using System.Linq;
using Exiled.Events.EventArgs;
using MEC;
using InventorySystem.Items;
using Exiled.API.Features.Items;
using Exiled.API.Features;
using UnityEngine;

namespace ExtraAdditions.FlashlightBattery
{
	class EventHandlers
	{
		private Dictionary<ItemBase, BatteryComponent> heldFlashlights = new Dictionary<ItemBase, BatteryComponent>();
		private Dictionary<Pickup, FlashlightData> droppedFlashlights = new Dictionary<Pickup, FlashlightData>();
		internal static Dictionary<Player, CoroutineHandle> flashlightHints = new Dictionary<Player, CoroutineHandle>();

		internal void OnRoundRestart()
		{
			heldFlashlights.Clear();
			droppedFlashlights.Clear();
			Timing.KillCoroutines(flashlightHints.Values.ToArray());
			flashlightHints.Clear();
		}

		internal void OnChangingItem(ChangingItemEventArgs ev)
		{
			if (ev.NewItem.Type == ItemType.Flashlight && heldFlashlights.ContainsKey(ev.NewItem.Base))
			{
				BatteryComponent component = heldFlashlights[ev.NewItem.Base];
				if (component != null)
				{
					if (component.IsBatteryDead())
					{
						Timing.CallDelayed(0.2f, () => component.TurnOffFlashlight());
						Plugin.AccessHintSystem(ev.Player, $"{new string('\n', Plugin.singleton.Config.FlashlightHintTextLower)}{Plugin.singleton.Translation.FlashlightIsDead}", Plugin.singleton.Config.DeadFlashlightHintTime);
					}
					else
					{
						component.SetDraining(true);
						if (!flashlightHints.ContainsKey(ev.Player))
						{
							flashlightHints.Add(ev.Player, Timing.RunCoroutine(ShowHint(ev.Player, component)));
						}
						else
						{
							Timing.KillCoroutines(flashlightHints[ev.Player]);
							flashlightHints[ev.Player] = Timing.RunCoroutine(ShowHint(ev.Player, component));
						}
					}
				}
			}
		}

		internal void OnToggleFlashlight(TogglingFlashlightEventArgs ev)
		{
			if (heldFlashlights.ContainsKey(ev.Flashlight.Base))
			{
				BatteryComponent component = heldFlashlights[ev.Flashlight.Base];
				if (component.IsBatteryDead() && ev.NewState)
				{
					ev.NewState = false;
					Plugin.AccessHintSystem(ev.Player, $"{new string('\n', Plugin.singleton.Config.FlashlightHintTextLower)}{Plugin.singleton.Translation.FlashlightIsDead}", Plugin.singleton.Config.DeadFlashlightHintTime);
				}
				else
				{
					component.SetDraining(ev.NewState);
					if (ev.NewState)
					{
						flashlightHints.Add(ev.Player, Timing.RunCoroutine(ShowHint(ev.Player, component)));
					}
					else if (flashlightHints.ContainsKey(ev.Player))
					{
						Timing.KillCoroutines(flashlightHints[ev.Player]);
						flashlightHints.Remove(ev.Player);
					}
				}
			}
		}

		internal void OnDroppingItem(DroppingItemEventArgs ev)
		{
			if (ev.Item.Type == ItemType.Flashlight)
			{
				if (heldFlashlights.ContainsKey(ev.Item.Base))
				{
					ev.IsAllowed = false;
					BatteryComponent component = heldFlashlights[ev.Item.Base];
					droppedFlashlights.Add(Item.Create(ItemType.Flashlight).Spawn(ev.Player.Position), new FlashlightData()
					{
						battery = component.GetRemainingBattery(),
						isDead = component.IsBatteryDead()
					});
					heldFlashlights.Remove(ev.Item.Base);
					ev.Player.RemoveItem(ev.Item);
					if (flashlightHints.ContainsKey(ev.Player))
					{
						Timing.KillCoroutines(flashlightHints[ev.Player]);
						flashlightHints.Remove(ev.Player);
					}
				}
			}
		}

		internal void OnPickingUpItem(PickingUpItemEventArgs ev)
		{
			if (ev.Pickup.Type == ItemType.Flashlight)
			{
				ev.IsAllowed = false;
				ItemBase b = ev.Player.AddItem(ItemType.Flashlight).Base;
				BatteryComponent component = b.gameObject.AddComponent<BatteryComponent>();
				if (droppedFlashlights.ContainsKey(ev.Pickup))
				{
					FlashlightData data = droppedFlashlights[ev.Pickup];
					component.Init(ev.Player, data.battery, data.isDead);
				}	
				else
				{
					component.Init(ev.Player);
				}
				heldFlashlights.Add(b, component);
				ev.Pickup.Destroy();
			}
		}

		internal void OnSpawn(SpawningEventArgs ev)
		{
			Timing.CallDelayed(0.1f, () =>
			{
				foreach (ItemBase item in ev.Player.Inventory.UserInventory.Items.Values)
				{
					if (item.ItemTypeId == ItemType.Flashlight && !heldFlashlights.ContainsKey(item))
					{
						BatteryComponent component = item.gameObject.AddComponent<BatteryComponent>();
						component.Init(ev.Player);
						heldFlashlights.Add(item, component);
					}
				}
			});
		}

		private IEnumerator<float> ShowHint(Player player, BatteryComponent component)
		{
			while (true)
			{
				int battery = (int)(component.GetRemainingBattery() / component.GetMaxBattery() * 100f);
				string bString = $"{new string('\n', Plugin.singleton.Config.FlashlightHintTextLower)}{Plugin.singleton.Translation.FlashlightBattery.Replace("{percent}", Mathf.Clamp(battery, 0f, 100f).ToString())}";
				if (battery <= 25)
				{
					bString = bString.Insert(Plugin.singleton.Config.FlashlightHintTextLower, battery <= 10 ? "<color=red>" : "<color=yellow>");
					bString += "</color>";
				}
				Plugin.AccessHintSystem(player, bString, 1f);
				yield return Timing.WaitForSeconds(1f);
			}
		}
	}

	class FlashlightData
	{
		public float battery;
		public bool isDead;
	}
}
