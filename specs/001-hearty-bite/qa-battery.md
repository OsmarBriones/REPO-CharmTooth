# QA battery: Hearty Bite (CharmTooth)

Deploy: Pockets, CharmCore and CharmTooth are in the r2modman `Debug` profile (compare hashes with the builds). Restart the game first.

## Quick start with debug mode
Set `DebugMode = true` in the `Debug` section of `BepInEx/config/com.osmar.CharmTooth.cfg` (off by default). When a level starts, a Tooth is placed on the floor about 2.5 m in front of you; the log then also shows the detailed lines. Any level works. To get another one at any moment, set `SpawnNow = true` in the same section while a level is running: the valuable is placed near you and the switch turns itself back off (the file is re-read about once a second).

## Single player
- [ ] Log shows `Charm registered: Tooth - Hearty Bite` and `CharmTooth 1.0.0 loaded!`.
- [ ] Store the valuable in a slot: the slot shows its picture and value.
- [ ] Hold a food valuable (the pills are a good test) while the Tooth is pocketed and health is below full; press E: the food vanishes, health rises by 1 per $100 of its value (at least 1), the Tooth loses about 20% of its value, the slot shows the new value.
- [ ] Press E twice quickly: the heal happens once.
- [ ] At full health, press E holding food: nothing happens, the food stays. With `EatAtFullHealth = 1` it is eaten with no heal.
- [ ] Press E holding a non-food valuable: nothing happens.
- [ ] Without the Tooth in the pocket, E on food does nothing.
- [ ] Add a name to `FoodNames`: after a restart that valuable can be eaten. A name in a different case or with `(Clone)` also matches.
- [ ] `HealPerHundredDollars = 0`: food is not eaten. `= 25`: a $400 food heals up to full.
- [ ] Take the valuable out: the power stops.
- [ ] Use it until the valuable breaks: the slot empties, the power stops, no errors in the log.

## Settings
- [ ] `WearPercent = 0`: no wear on the Tooth.
- [ ] Charm `Enabled = false`: no effect and no wear. CharmCore `Enabled = false`: nothing works.

## Multiplayer (host and client)
- [ ] The effect works on the client: the food disappears on both machines, the client heals, the Tooth's value drops on both. The client's grab is released cleanly and the log has no errors.
- [ ] The host eating works the same. A client without the Tooth cannot eat.
- [ ] Host settings override the client's.
- [ ] Client leaves and rejoins: no stuck veil.

Log patterns: `Charm registered`, `threw`, `Exception`.
