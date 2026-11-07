using System.Collections.Generic;
using CharmTooth.Domain;
using CharmTooth.Net;
using UnityEngine;

namespace CharmTooth
{
	/// <summary>
	/// Hearty Bite, owner side: while the Tooth is pocketed, pressing E (the game's Interact key) with a food valuable in hand
	/// eats it. The bite heals the owner (health is owner-only) and asks the host to destroy the valuable
	/// (<see cref="FoodHost"/>). The Tooth wears by <c>WearPercent</c> per bite.
	/// </summary>
	internal sealed class HeartyBite : MonoBehaviour
	{
		/// <summary>How long a valuable stays on the "already eaten" list, so a double press cannot heal twice.</summary>
		private const float EatenMemorySeconds = 5f;

		private readonly Dictionary<int, float> eaten = new Dictionary<int, float>();
		private readonly List<int> expired = new List<int>();

		internal ToothCharm Charm { get; set; } = null!;

		private void Update()
		{
			if (Charm == null || !Charm.IsActive || !SemiFunc.InputDown(InputKey.Interact))
			{
				return;
			}

			TryEat();
		}

		private void TryEat()
		{
			PlayerController? controller = PlayerController.instance;
			PhysGrabber? grabber = PhysGrabber.instance;
			PlayerAvatar? avatar = PlayerAvatar.instance;
			if (controller == null || grabber == null || avatar == null || avatar.playerHealth == null)
			{
				return;
			}

			if (controller.InputDisableTimer > 0f || avatar.isDisabled)
			{
				Charm.LogDebug("Hearty Bite: E ignored, input is disabled or the player is dead");
				return;
			}

			PhysGrabObject? held = grabber.grabbed ? grabber.grabbedPhysGrabObject : null;
			if (held == null)
			{
				return;
			}

			ValuableObject? valuable = held.GetComponent<ValuableObject>();
			if (valuable == null || !FoodList.IsFood(Charm.Foods, held.name))
			{
				Charm.LogDebug($"Hearty Bite: E pressed holding {held.name}, which is not food");
				return;
			}

			int key = FoodHost.KeyOf(held);
			ForgetOldBites();
			if (eaten.ContainsKey(key))
			{
				Charm.LogDebug($"Hearty Bite: {held.name} (key {key}) is already being eaten");
				return;
			}

			PlayerHealth health = avatar.playerHealth;
			if (!HealMath.MayEat(health.health, health.maxHealth, Charm.EatAtFullHealth.Value >= 0.5f))
			{
				Charm.LogDebug($"Hearty Bite: health is full ({health.health}/{health.maxHealth}), the food is kept");
				return;
			}

			int amount = HealMath.HealAmount(valuable.dollarValueCurrent, Charm.HealPerHundredDollars.Value);
			if (amount <= 0)
			{
				Charm.LogDebug($"Hearty Bite: {held.name} worth {valuable.dollarValueCurrent:0} would heal nothing, the food is kept");
				return;
			}

			if (!Charm.IsReady)
			{
				Charm.LogDebug("Hearty Bite: the Tooth is not ready");
				return;
			}

			eaten[key] = Time.time + EatenMemorySeconds;
			Charm.LogDebug($"Hearty Bite: eating {held.name} (key {key}, value {valuable.dollarValueCurrent:0}), healing {amount} ({health.health}/{health.maxHealth})");

			// Order matters: the request goes out while the Tooth is still active; the wear that follows may break it.
			FoodHost.Remember(key, held);
			Charm.SendToHost(ToothCharm.EatAction, key);
			FoodHost.Forget(key);
			Charm.TryUse();
			health.Heal(amount, true);
		}

		/// <summary>Drops the entries of valuables eaten long ago.</summary>
		private void ForgetOldBites()
		{
			expired.Clear();
			foreach (KeyValuePair<int, float> entry in eaten)
			{
				if (entry.Value <= Time.time)
				{
					expired.Add(entry.Key);
				}
			}

			foreach (int key in expired)
			{
				eaten.Remove(key);
			}
		}
	}
}
