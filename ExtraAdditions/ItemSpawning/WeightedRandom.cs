using System;
using System.Collections.Generic;

namespace ExtraAdditions.ItemSpawning
{
    public class WeightedRandomBag<T>
    {
        private List<Entry> entries = new List<Entry>();
        private double accumulatedWeight;
        private Random rand = new Random();

        private class Entry
        {
            public double accumulatedWeight;
            public T obj;
        }

        public void addEntry(T obj, double weight)
        {
            accumulatedWeight += weight;
            Entry e = new Entry();
            e.obj = obj;
            e.accumulatedWeight = accumulatedWeight;
            entries.Add(e);
        }

        public void Clear()
		{
            accumulatedWeight = 0d;
            entries.Clear();
		}

        public T getRandom()
        {
            double r = rand.NextDouble() * accumulatedWeight;

            foreach (Entry entry in entries)
            {
                if (entry.accumulatedWeight >= r)
                {
                    return entry.obj;
                }
            }
            return default(T);
        }
    }
}
