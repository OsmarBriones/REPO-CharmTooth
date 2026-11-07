using System.Collections.Generic;
using CharmTooth.Domain;
using Photon.Pun;
using Pockets.Api;
using UnityEngine;

namespace CharmTooth.Net
{
	/// <summary>
	/// Host side of Hearty Bite, and the identification of a valuable across machines. Self-contained so the Gold Tooth mod can mirror it.
	/// Only the host may destroy a valuable, so the owner sends the valuable's key and the host checks the bite is legitimate
	/// (the sender carries the Tooth, the valuable is food, the sender is holding it, nobody has it in a pocket) before destroying it.
	/// </summary>
	internal static class FoodHost
	{
		/// <summary>Valuables the owner is eating right now, by key. Used alone (and as host) where the key cannot be looked up through Photon.</summary>
		private static readonly Dictionary<int, PhysGrabObject> Pending = new Dictionary<int, PhysGrabObject>();

		/// <summary>Opaque key of a valuable: the Photon view id in multiplayer, the Unity instance id alone (where every view id is 0).</summary>
		internal static int KeyOf(PhysGrabObject grabObject)
		{
			return SemiFunc.IsMultiplayer() ? grabObject.photonView.ViewID : grabObject.gameObject.GetInstanceID();
		}

		/// <summary>Owner side: remembers the valuable so a request handled on this same machine can find it.</summary>
		internal static void Remember(int key, PhysGrabObject grabObject)
		{
			Pending[key] = grabObject;
		}

		/// <summary>Owner side: forgets a remembered valuable once the request has been handled.</summary>
		internal static void Forget(int key)
		{
			Pending.Remove(key);
		}

		private static PhysGrabObject? Resolve(int key)
		{
			if (SemiFunc.IsMultiplayer())
			{
				PhotonView view = PhotonView.Find(key);
				return view != null ? view.GetComponent<PhysGrabObject>() : null;
			}

			return Pending.TryGetValue(key, out PhysGrabObject? found) && found != null ? found : null;
		}

		/// <summary>Runs on the host (or alone). Validates the request and destroys the eaten valuable.</summary>
		internal static void HandleEat(ToothCharm charm, PlayerAvatar sender, object[] args)
		{
			if (args.Length < 1 || args[0] is not int key)
			{
				charm.LogDebug("Host: eat request without a valuable key, ignored");
				return;
			}

			PhysGrabObject? grabObject = Resolve(key);
			if (grabObject == null)
			{
				charm.LogDebug($"Host: eat request for key {key} ignored, the valuable was not found");
				return;
			}

			ValuableObject? valuable = grabObject.GetComponent<ValuableObject>();
			if (valuable == null || !FoodList.IsFood(charm.Foods, grabObject.name))
			{
				charm.LogDebug($"Host: eat request ignored, {grabObject.name} is not food");
				return;
			}

			if (grabObject.dead || PocketsApi.IsPocketed(valuable))
			{
				charm.LogDebug($"Host: eat request ignored, {grabObject.name} is already gone or pocketed");
				return;
			}

			PhysGrabber? grabber = sender.physGrabber;
			if (grabber == null || !grabObject.playerGrabbing.Contains(grabber))
			{
				charm.LogDebug($"Host: eat request ignored, the sender is not holding {grabObject.name}");
				return;
			}

			charm.LogDebug($"Host: {grabObject.name} (key {key}) eaten, destroying it");

			// The sender lets go first (the vanilla way for the host to make a player release), then the valuable disappears on every machine.
			grabber.OverrideGrabRelease(-1);
			grabObject.DestroyPhysGrabObject();
		}
	}
}
