# CharmTooth Architecture

A Charm mod (layer 3 of the PocketCharms family): one valuable, one power. It depends on **Pockets** (storage) and **CharmCore** (registry, wear, cooldown display, host settings, debug mode).

## The power: Hearty Bite

While the Tooth is pocketed, pressing E while holding a food valuable eats it: the valuable is destroyed and the owner heals.

- `ToothCharm : Charm` registers the prefab name `Valuable Museum Tooth`, the defaults (wear 20%, no cooldown) and any extra host-synced settings.
- `HeartyBite` (a `MonoBehaviour` on the plugin object, owner side) polls `SemiFunc.InputDown(InputKey.Interact)` while `Charm.IsActive`. The held object is `PhysGrabber.instance.grabbedPhysGrabObject` while `PhysGrabber.grabbed`. It checks food (name allowlist), full health, heal amount > 0 and `Charm.IsReady`, then sends `SendToHost("eat", key)`, calls `Charm.TryUse()` (wear) and `PlayerHealth.Heal(amount, true)`. A short-lived set of eaten keys (5 s) stops a double press from healing twice.
- `Net/FoodHost` (host side, self-contained so Gold Tooth can mirror it) handles `OnHostRequest`: it finds the valuable (Photon view id in multiplayer; a remembered reference alone, where view ids are 0), checks it is food, not dead, not pocketed (`PocketsApi.IsPocketed`) and in the sender's `PhysGrabObject.playerGrabbing`; then it makes the sender let go (`PhysGrabber.OverrideGrabRelease(-1)`, the vanilla host-side release) and calls `PhysGrabObject.DestroyPhysGrabObject()`.
- `Domain/HealMath` (heal amount, full health rule) and `Domain/FoodList` (parsing and matching names, `(Clone)` stripped, case-insensitive) are game-free and unit tested.

## Settings

| Setting | Type | Range | Default | Notes |
|---|---|---|---|---|
| `WearPercent` | float, % of the Tooth's original value per bite | 0 to 100 | 20 | CharmCore |
| `HealPerHundredDollars` | float, HP per $100 | 0 to 25 | 1 | host-synced; 0 = nothing heals, so food is kept |
| `EatAtFullHealth` | 0/1 flag | 0 to 1 | 0 | host-synced; at 1 food is eaten at full health with no heal |
| `FoodNames` | string list, local | n/a | the eight food prefabs | read once at startup |

No cooldown. Heal = value / 100 * rate, rounded half up, at least 1 when both are above 0, then clamped by the game to max health.

## Decisions

- `DestroyPhysGrabObject` rather than `impactDetector.DestroyObject`: the latter only marks the object dead and the game destroys it after everyone lets go, and it plays break effects. Eating should remove it at once, with no loss effect. `DestroyPhysGrabObject` is what vanilla calls once the grab ends, and the object's `OnDestroy` removes it from the haul list.
- The owner heals and wears locally and does not wait for the host: health is owner-only. If the host rejects a request (modified client, race), the owner has healed for nothing; the host is the authority on the valuable only.
- Pills: the game has one pills prefab (`Valuable Arctic Pills`), so "all sizes" is one name.
- Food worth 0, or a rate of 0, is not eaten (nothing would heal). At full health the food is kept unless `EatAtFullHealth` is 1.
- `FoodNames` is a plain Bind (a list of names), not a `CharmSetting`; it is local, not host-synced.

## Game members used

`SemiFunc.InputDown`, `InputKey.Interact`, `PlayerController.InputDisableTimer`, `PhysGrabber.instance/grabbed/grabbedPhysGrabObject/OverrideGrabRelease`, `PhysGrabObject.playerGrabbing/dead/photonView/DestroyPhysGrabObject`, `ValuableObject.dollarValueCurrent`, `PlayerAvatar.instance/playerHealth/physGrabber/isDisabled`, `PlayerHealth.health/maxHealth/Heal`. Checked by `CharmTooth.Tests/GameContractTests.cs`.

## Debug mode

Provided by CharmCore: the `Debug.DebugMode` setting (off by default) places the valuable on the floor at the start of each level and enables `Charm.LogDebug` output.

## Known limits

- Looked up by exact prefab name; a game update that renames the prefab needs a mod update. The same goes for the food names.
- E may also be used by other things (other mods, an item the player holds); the bite triggers only with a food valuable in hand.
- Not verified in game: the E press while holding, the host release plus destroy of a held valuable for a remote client, and the heal effect.
