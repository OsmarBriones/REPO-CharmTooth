using System;
using System.Collections.Generic;

namespace CharmTooth.Domain
{
	/// <summary>
	/// The names of the valuables that count as food. The game has no food flag, so food is a list of prefab names, matched
	/// case-insensitively with the "(Clone)" suffix of spawned objects ignored. Game-free and unit tested.
	/// </summary>
	internal static class FoodList
	{
		/// <summary>Default list: the food valuables of the game (all sizes of the pills are the single "Arctic Pills" prefab).</summary>
		internal const string DefaultNames = "Valuable Museum Toast, Valuable Museum Banana Bow, Valuable Wizard Red Mushroom, Valuable Museum Gumball, Valuable Museum Egg, Valuable Museum Gem Burger, Valuable Museum Milk, Valuable Arctic Pills";

		private const string CloneSuffix = "(Clone)";

		/// <summary>Splits a comma separated list into normalised names. Empty entries are dropped; null or empty gives an empty set.</summary>
		internal static HashSet<string> Parse(string? text)
		{
			HashSet<string> names = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			if (string.IsNullOrWhiteSpace(text))
			{
				return names;
			}

			foreach (string part in text!.Split(','))
			{
				string name = Normalize(part);
				if (name.Length > 0)
				{
					names.Add(name);
				}
			}

			return names;
		}

		/// <summary>Trims the name and removes every trailing "(Clone)".</summary>
		internal static string Normalize(string? name)
		{
			string result = (name ?? string.Empty).Trim();
			while (result.EndsWith(CloneSuffix, StringComparison.OrdinalIgnoreCase))
			{
				result = result.Substring(0, result.Length - CloneSuffix.Length).TrimEnd();
			}

			return result;
		}

		/// <summary>True when <paramref name="objectName"/> (for example "Valuable Museum Toast(Clone)") is in the food set.</summary>
		internal static bool IsFood(ISet<string> foods, string? objectName)
		{
			string name = Normalize(objectName);
			return name.Length > 0 && foods.Contains(name);
		}
	}
}
