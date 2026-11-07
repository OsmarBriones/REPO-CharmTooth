using BepInEx;
using CharmCore.Api;
using CharmTooth.Domain;
using UnityEngine;

namespace CharmTooth
{
	[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
	[BepInDependency("com.osmar.Pockets", BepInDependency.DependencyFlags.HardDependency)]
	[BepInDependency("com.osmar.CharmCore", BepInDependency.DependencyFlags.HardDependency)]
	public class CharmToothPlugin : BaseUnityPlugin
	{
		public const string PluginGuid = "com.osmar.CharmTooth";
		public const string PluginName = "CharmTooth";
		public const string PluginVersion = "1.0.0";

		internal static new BepInEx.Logging.ManualLogSource Logger { get; private set; } = null!;

		private void Awake()
		{
			Logger = base.Logger;

			// Prevent the plugin from being deleted
			this.gameObject.transform.parent = null;
			this.gameObject.hideFlags = HideFlags.HideAndDontSave;

			// Registering binds Enabled, WearPercent, CooldownSeconds and the Debug section (DebugMode) in this mod's config file.
			ToothCharm charm = new ToothCharm();
			CharmsApi.Register(charm, Config);

			// The food list is a name allowlist read once at startup (the game has no food flag).
			string foodNames = Config.Bind<string>("Charm.Tooth", "FoodNames", FoodList.DefaultNames, "Comma separated prefab names of the valuables that count as food, matched without regard to capitals and ignoring '(Clone)'. Read when the game starts; local setting.").Value;
			charm.Foods = FoodList.Parse(foodNames);

			this.gameObject.AddComponent<HeartyBite>().Charm = charm;

			Logger.LogInfo($"{PluginName} {PluginVersion} loaded!");
		}
	}
}
