# CLAUDE.md — Fables Unite

Instructions and conventions for any Claude Code session working in this repo. Read this first, then `docs/GDD.md` for the full design.

## What this project is
Fables Unite — a mobile (iOS/Android) **lane-defense + creature-collection** game built in **Unity 6.4 (URP), portrait**. Solo developer. Free-to-play, "fair F2P." The player collects **Fables** (heroes + creatures) and deploys squads to defend lanes against Dr. Vex's army; between battles they pull the **Growth Tree** (the loot engine) and upgrade Fables.

Currently a **client-side vertical slice** (Phase 1). The full status of every system — built vs. planned vs. deferred — lives in `Fables-Unite-GDD-v2.md` Section 0. Trust that document over any assumptions.

## Where things live
- `Assets/_Project/Scripts/` — all C# gameplay scripts
- `Assets/_Project/Data/` — ScriptableObject assets (Fables, FableDatabase, TreeConfig)
- `Assets/_Project/Prefabs/` and `Assets/_Project/UI/` — prefabs
- `Assets/_Project/Scenes/` — Home, Tree, Battle, Camp, Roster, Tower, TurfWars
- New scripts go in `Scripts/`; new data goes in `Data/`. Do not scatter files elsewhere.

## Architecture conventions (follow these; don't reinvent)
- **Content is data, not code.** Fables, tree spots, and the database are ScriptableObjects. A new Fable or reward = a new asset, never a new class.
- **Save data stores id strings, not asset references.** `OwnedFable.definitionId` is a string resolved via `FableDatabase.Get(id)`. Keep it this way so `JsonUtility` serialization stays clean.
- **One source of truth for player state:** everything the player owns lives in `GameData.I.player` (`PlayerData`). All currency/roster changes funnel through it, then call `GameData.I.Save()`.
- **Persistent singletons:** `GameData` and `SceneLoader` are `DontDestroyOnLoad`. Don't create competing managers.
- **Pure logic in static helpers** with no scene dependencies: `ElementChart`, `FableStats`, `FableUpgrade`. Put new pure/testable logic here, not in MonoBehaviours.
- **Navigation:** each mode is its own scene loaded via `SceneLoader`; sub-views (like Fable detail) are panels, not scenes.
- Match the existing code style (naming, spacing, comment tone). Read a neighboring script before writing a new one.

## Direction / guardrails (important)
- The loot engine is the **Growth Tree**. The old "Skee Vault" / skee-ball design is **retired** — do not reintroduce it.
- The economy is **client-side / local JSON for now on purpose.** Server-authority via Unity Gaming Services is **deferred** — do NOT add UGS, Cloud Code, or networking unless explicitly asked.
- These are **planned but intentionally not built yet** — don't build them unless the task says so: multiplier toggle (1×–5×) and Fever Meter on the Tree, Feed Rank (E→S), Global Level, 5th rarity (Rainbow/Mythic), evolution trials, star badge tiers, Gems currency, Feed Kitchen / Decoration buildings.

## How to work in this repo
- **Plan before non-trivial changes.** State which files you'll touch and why; wait for approval before large edits.
- **Keep changes tightly scoped.** If a task is about the Tree, don't refactor combat. Don't do drive-by rewrites.
- **Write tests for pure logic** (Unity Test Framework, EditMode) when you add or change `ElementChart`, `FableStats`, `FableUpgrade`, or the Growth Tree pull math. These have no scene dependencies and are cheap to protect.
- **Editor steps are the human's job.** You can write and edit scripts freely. Anything that requires the Unity Editor (wiring Inspector references, creating GameObjects/prefabs, laying out Canvases, assigning ScriptableObject fields) — list those steps explicitly and clearly for the human to do, unless a Unity MCP tool is connected and the task says to use it.
- **Never edit files under any read-only path.** Copy out first if needed.

## Gotchas
- Unity `.unity` (scene) and `.prefab` files are large YAML and diff poorly in git. Before making Editor/scene changes (including via MCP), assume the human has committed, so a revert is always possible.
- Some MCP Editor operations don't work while Unity is in Play mode.
- Known data issue to be aware of: in `TreeConfig`, the "Fable Shards (big)" spot's `type` was set to RaidCoins (2) rather than FableShards (3) — confirm intent before "fixing."
