using System;

namespace CharmTooth.Domain
{
	/// <summary>Game-free rules of Hearty Bite: how much health a bite of a given value restores. Unit tested.</summary>
	internal static class HealMath
	{
		/// <summary>
		/// Health points restored by eating a valuable worth <paramref name="value"/> dollars, at <paramref name="hpPerHundredDollars"/>
		/// points per $100. Rounded to the nearest whole point (halves go up). When both the value and the rate are above 0 the result
		/// is at least 1; otherwise it is 0 (nothing to heal). Never negative.
		/// </summary>
		internal static int HealAmount(float value, float hpPerHundredDollars)
		{
			if (!(value > 0f) || !(hpPerHundredDollars > 0f))
			{
				return 0;
			}

			double points = value / 100.0 * hpPerHundredDollars;
			if (points >= int.MaxValue)
			{
				return int.MaxValue;
			}

			int rounded = (int)Math.Round(points, MidpointRounding.AwayFromZero);
			return rounded < 1 ? 1 : rounded;
		}

		/// <summary>True when the bite may go ahead: below full health, or at full health only when the setting allows it.</summary>
		internal static bool MayEat(int health, int maxHealth, bool eatAtFullHealth)
		{
			return eatAtFullHealth || health < maxHealth;
		}
	}
}
