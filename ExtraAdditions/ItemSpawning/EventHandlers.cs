using Exiled.API.Features;
using Exiled.API.Features.Items;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ExtraAdditions.ItemSpawning
{
	class EventHandlers
	{
		private const int WorldMask = 1207976449;
		private const int MaxBenchRange = 8;
		const int benchPercentage = 100;

		private float dir(float w, float x, float y, float z)
		{
			float m = Math.Max(w, Math.Max(x, Math.Max(y, z)));
			if (w == m) return 0f;
			else if (x == m) return 90f;
			else if (y == m) return 180f;
			else return 270f;
		}

		internal void OnRoundStart()
		{
			List<Room> rooms = Room.List.Where(x => x.name.Contains("EZ_Curve")).ToList();
			rooms.ShuffleList();
			//for (int i = 0; i < rooms.Count / (float)benchPercentage; i++)
			for (int i = 0; i < rooms.Count; i++)
			{
				Room room = rooms[i];
				Vector3 pos = room.transform.position;
				pos.y += 2;
				Physics.Raycast(pos, Vector3.forward, out RaycastHit hit1, WorldMask);
				Physics.Raycast(pos, Vector3.right, out RaycastHit hit2, WorldMask);
				Physics.Raycast(pos, Vector3.back, out RaycastHit hit3, WorldMask);
				Physics.Raycast(pos, Vector3.left, out RaycastHit hit4, WorldMask);

				List<Vector3> dir = new List<Vector3>();
				if (hit1.distance < MaxBenchRange) dir.Add(Vector3.forward);
				if (hit2.distance < MaxBenchRange) dir.Add(Vector3.right);
				if (hit3.distance < MaxBenchRange) dir.Add(Vector3.back);
				if (hit4.distance < MaxBenchRange) dir.Add(Vector3.left);

				foreach (Vector3 location in dir)
				{
					Vector3 bench = pos + location * 5;
					Vector3 pos1 = Quaternion.Euler(0, 90, 0) * location;
					Vector3 pos2 = Quaternion.Euler(0, -90, 0) * location;
					Physics.Raycast(bench, pos1, out RaycastHit hit5, WorldMask);
					Physics.Raycast(bench, pos2, out RaycastHit hit6, WorldMask);
					if (hit5.distance > hit6.distance)
					{
						Item.Create(ItemType.Flashlight).Spawn(bench + pos1 * 0.5f);
						Item.Create(ItemType.Flashlight).Spawn(bench + pos1 * 1.4f);
					}
					else
					{
						Item.Create(ItemType.Flashlight).Spawn(bench + pos2 * 0.5f);
						Item.Create(ItemType.Flashlight).Spawn(bench + pos2 * 1.4f);
					}
				}
			}
		}
	}
}
