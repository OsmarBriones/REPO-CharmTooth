# Charm Tooth

Hearty Bite: while the Tooth is in your pocket, hold a food valuable and press E to eat it and heal.

> **Everyone in the lobby must install this mod, together with Pockets and CharmCore.** The power changes what your own character can do and its state shows on your inventory slot, so friends without the mods would see an empty slot and be unable to use the pocket.

## How to Use

1. Find the **Tooth** valuable and store it in a free inventory slot with [Pockets](https://thunderstore.io/c/repo/p/OsmarBriones/Pockets/) (grab it, press the slot key).
2. Pick up a food valuable (toast, banana bow, red mushroom, gumball, egg, gem burger, milk or pills) and, while holding it, press **E**. You eat it: the valuable is gone (and its value with it) and you heal 1 health point for every $100 it was worth. The Tooth loses a little of its value with every bite.
3. Take the valuable out of the pocket and the power stops.

## Features

- **Hearty Bite:** food becomes health. A $500 egg heals 5 points; anything worth something heals at least 1. At full health the food is kept (you can change that).
- **It costs the valuable:** each use wears away a share of its value, and a worn-out valuable breaks.
- **Everything is configurable:** wear, how much each $100 heals, whether you may eat at full health, and which valuables count as food. The host's choices apply to everyone.

## Requirements
- [BepInEx Pack for R.E.P.O.](https://thunderstore.io/c/repo/p/BepInEx/BepInExPack/)
- [Pockets](https://thunderstore.io/c/repo/p/OsmarBriones/Pockets/)
- [CharmCore](https://thunderstore.io/c/repo/p/OsmarBriones/CharmCore/)

## Installation
1. Install through r2modman / Thunderstore Mod Manager (Pockets and CharmCore come with it), or place `CharmTooth.dll` next to `Pockets.dll` and `CharmCore.dll` in `BepInEx/plugins`. **All players need it.**
2. Launch the game once; the configuration file is created inside `BepInEx/config`.

## Configuration
Settings are in `com.osmar.CharmTooth.cfg` inside `BepInEx/config`, section `Charm.Tooth`. The host's choices apply to everyone.

- `Enabled` (default `true`): turn this charm on or off.
- `WearPercent` (default `20`): share of the Tooth's original value lost on each bite.
- `HealPerHundredDollars` (default `1`, 0 to 25): health points restored per $100 of the food's current value.
- `EatAtFullHealth` (default `0`, 0 or 1): 1 lets you eat food at full health (it is consumed with nothing healed).
- `FoodNames` (local, read at game start): comma separated list of the valuables that count as food, by their internal names (for example `Valuable Museum Toast`).

Section `Debug`: `DebugMode` (default `false`) places a Tooth on the floor in front of you when a level starts (host or singleplayer) and writes extra log lines. For testing. `SpawnNow` (default `false`, needs `DebugMode`): set it to `true` while a level is running to place one right away; it switches itself back off.

## Issues & Bug Reports
The official way to report issues, suggest improvements, or submit feedback is by opening an issue on the official GitHub repository:
👉 [GitHub Issues](https://github.com/OsmarBriones/REPO-CharmTooth/issues)

## Credits
Developed by **Osmar Briones**
