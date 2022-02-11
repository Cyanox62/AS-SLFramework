using Exiled.Events.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacilityGenerators
{
	class EventHandlers
	{
		internal void OnSpawn(SpawningEventArgs ev)
		{
			if (ev.Player.Team == Team.MTF)
			{
				ev.Player.AddItem(ItemType.Flashlight);
			}
		}

		internal void OnTesla(TriggeringTeslaEventArgs ev)
		{

		}
	}
}
