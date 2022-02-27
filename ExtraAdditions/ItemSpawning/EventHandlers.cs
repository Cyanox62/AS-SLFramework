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
		private const int MaxWallThreshold = 8;

		internal static WeightedRandomBag<ItemType> itemDrops = new WeightedRandomBag<ItemType>();

		internal List<Vector3> GetPositionsUnder(Room room, float threshold)
		{
			Vector3 pos = room.transform.position;
			pos.y += 2;
			Physics.Raycast(pos, Vector3.forward, out RaycastHit hit1, WorldMask);
			Physics.Raycast(pos, Vector3.right, out RaycastHit hit2, WorldMask);
			Physics.Raycast(pos, Vector3.back, out RaycastHit hit3, WorldMask);
			Physics.Raycast(pos, Vector3.left, out RaycastHit hit4, WorldMask);

			List<Vector3> dir = new List<Vector3>();
			if (hit1.distance < threshold) dir.Add(Vector3.forward);
			if (hit2.distance < threshold) dir.Add(Vector3.right);
			if (hit3.distance < threshold) dir.Add(Vector3.back);
			if (hit4.distance < threshold) dir.Add(Vector3.left);

			return dir;
		}

		internal void OnRoundStart()
		{
			foreach (Room room in Room.List.Where(x => x.Name.Contains("EZ"))) Log.Warn(room.Name);

			List<Vector3> spawnPositions = new List<Vector3>();

			List<Room> curveRooms = Room.List.Where(x => x.name.Contains("EZ_Curve")).ToList();
			curveRooms.ShuffleList();
			//for (int i = 0; i < curveRooms.Count / (float)benchPercentage; i++)
			for (int i = 0; i < curveRooms.Count; i++)
			{
				Room room = curveRooms[i];
				Vector3 pos = room.transform.position;
				pos.y += 2;

				foreach (Vector3 location in GetPositionsUnder(room, MaxWallThreshold))
				{
					Vector3 bench = pos + location * 5;
					Vector3 pos1 = Quaternion.Euler(0, 90, 0) * location;
					Vector3 pos2 = Quaternion.Euler(0, -90, 0) * location;
					Physics.Raycast(bench, pos1, out RaycastHit hit5, WorldMask);
					Physics.Raycast(bench, pos2, out RaycastHit hit6, WorldMask);
					if (hit5.distance > hit6.distance)
					{
						spawnPositions.Add(bench + pos1 * 0.5f);
						spawnPositions.Add(bench + pos1 * 1.5f);
					}
					else
					{
						spawnPositions.Add(bench + pos2 * 0.5f);
						spawnPositions.Add(bench + pos2 * 1.5f);
					}
				}
			}

			List<Room> tRooms = Room.List.Where(x => x.name.Contains("EZ_ThreeWay")).ToList();
			tRooms.ShuffleList();
			//for (int i = 0; i < tRooms.Count / (float)benchPercentage; i++)
			for (int i = 0; i < tRooms.Count; i++)
			{
				Room room = tRooms[i];
				Vector3 pos = room.transform.position;
				pos.y += 2;

				foreach (Vector3 location in GetPositionsUnder(room, MaxWallThreshold))
				{
					Vector3 bench = pos + location * 5;
					Vector3 pos1 = Quaternion.Euler(0, 90, 0) * location;
					Vector3 pos2 = Quaternion.Euler(0, -90, 0) * location;
					spawnPositions.Add(bench + pos1 * 1f);
					spawnPositions.Add(bench + pos1 * 2f);
					spawnPositions.Add(bench + pos2 * 1f);
					spawnPositions.Add(bench + pos2 * 2f);
				}
			}

			for (int i = 0; i < spawnPositions.Count; i++)
			{
				if (UnityEngine.Random.Range(0, 100) < Plugin.singleton.Config.BenchSpawnChance)
				{
					Item.Create(itemDrops.getRandom()).Spawn(spawnPositions[i], Quaternion.Euler(UnityEngine.Random.Range(10f, 80f), UnityEngine.Random.Range(10f, 80f), UnityEngine.Random.Range(10f, 80f)));
				}
			}
		}
	}
}
