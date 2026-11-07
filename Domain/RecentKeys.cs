using System.Collections.Generic;

namespace CharmTooth.Domain
{
	/// <summary>
	/// A short-lived set of keys. Used to remember which valuable was just eaten, so pressing E again before the host has
	/// removed it does not heal twice. Time is passed in, so it is unit tested without a clock.
	/// </summary>
	internal sealed class RecentKeys
	{
		private readonly Dictionary<int, float> expiry = new Dictionary<int, float>();
		private readonly List<int> old = new List<int>();

		internal int Count => expiry.Count;

		/// <summary>Adds the key for <paramref name="seconds"/>. Returns false (and changes nothing) when it is already remembered.</summary>
		internal bool TryAdd(int key, float now, float seconds)
		{
			if (Contains(key, now))
			{
				return false;
			}

			expiry[key] = now + seconds;
			return true;
		}

		internal bool Contains(int key, float now)
		{
			return expiry.TryGetValue(key, out float until) && now < until;
		}

		internal void Remove(int key)
		{
			expiry.Remove(key);
		}

		/// <summary>Forgets the keys whose time is up.</summary>
		internal void Prune(float now)
		{
			if (expiry.Count == 0)
			{
				return;
			}

			old.Clear();
			foreach (KeyValuePair<int, float> entry in expiry)
			{
				if (now >= entry.Value)
				{
					old.Add(entry.Key);
				}
			}

			foreach (int key in old)
			{
				expiry.Remove(key);
			}
		}

		internal void Clear()
		{
			expiry.Clear();
		}
	}
}
