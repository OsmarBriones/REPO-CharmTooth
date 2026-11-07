using CharmCore.Api;

namespace CharmTooth
{
	/// <summary>Hearty Bite: press E on food to eat it and heal.</summary>
	internal sealed class ToothCharm : Charm
	{
		internal const string PrefabName = "Valuable Museum Tooth";

		internal ToothCharm()
			: base("Tooth", "Tooth - Hearty Bite", defaultWearPercent: 20f, defaultCooldownSeconds: null, PrefabName)
		{
			// Extra host-synced numbers for this power go here, for example:
			// Amount = AddSetting("Amount", 30f, 1f, 100f, "Description for the config file.");
		}
	}
}
