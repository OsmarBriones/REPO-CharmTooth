using System.Collections.Generic;
using CharmCore.Api;
using CharmTooth.Domain;
using CharmTooth.Net;

namespace CharmTooth
{
	/// <summary>Hearty Bite: press E on food to eat it and heal.</summary>
	internal sealed class ToothCharm : Charm
	{
		internal const string PrefabName = "Valuable Museum Tooth";

		/// <summary>Name of the request the owner sends to the host when a bite happens.</summary>
		internal const string EatAction = "eat";

		internal ToothCharm()
			: base("Tooth", "Tooth - Hearty Bite", defaultWearPercent: 20f, defaultCooldownSeconds: null, PrefabName)
		{
			HealPerHundredDollars = AddSetting("HealPerHundredDollars", 1f, 0f, 25f, "Health points restored for every $100 of the food's current value (rounded to the nearest point, at least 1 when the food is worth something). 0 means eating heals nothing.");
			EatAtFullHealth = AddSetting("EatAtFullHealth", 0f, 0f, 1f, "0 = Hearty Bite does nothing while your health is full, so food is not wasted. 1 = you may still eat food at full health (it is consumed and wears the Tooth, with nothing healed).");
		}

		/// <summary>Health points per $100 of the food's value (0 to 25).</summary>
		internal CharmSetting HealPerHundredDollars { get; }

		/// <summary>1 when food may be eaten at full health, 0 when not.</summary>
		internal CharmSetting EatAtFullHealth { get; }

		/// <summary>Prefab names that count as food; read once at startup from the config file.</summary>
		internal HashSet<string> Foods { get; set; } = FoodList.Parse(FoodList.DefaultNames);

		protected override void OnHostRequest(PlayerAvatar sender, string action, object[] args)
		{
			if (action == EatAction)
			{
				FoodHost.HandleEat(this, sender, args);
			}
			else
			{
				LogDebug($"Host: ignored unknown request '{action}'");
			}
		}
	}
}
