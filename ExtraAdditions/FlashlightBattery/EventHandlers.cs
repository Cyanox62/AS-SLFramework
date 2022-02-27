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
		private Dictionary<Pickup, float> droppedFlashlights = new Dictionary<Pickup, float>();

		internal void OnRoundRestart()
		{
			heldFlashlights.Clear();
			droppedFlashlights.Clear();
		}

		internal void OnDroppingItem(DroppingItemEventArgs ev)
		{
			if (ev.Item.Type == ItemType.Flashlight)
			{
				if (heldFlashlights.ContainsKey(ev.Item.Base))
				{
					ev.IsAllowed = false;
					droppedFlashlights.Add(Item.Create(ItemType.Flashlight).Spawn(ev.Player.Position), heldFlashlights[ev.Item.Base].GetRemainingBattery());
					heldFlashlights.Remove(ev.Item.Base);
					ev.Player.RemoveItem(ev.Item);
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
				component.Init(droppedFlashlights.ContainsKey(ev.Pickup) ? droppedFlashlights[ev.Pickup] : component.GetMaxBattery());
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
						component.Init();
						heldFlashlights.Add(item, component);
					}
				}
			});
		}
	}
}
