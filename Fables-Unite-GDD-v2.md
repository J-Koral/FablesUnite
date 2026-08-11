# FABLES UNITE — Game Design Document

**Version 2.0 — August 2026**
**Platform:** iOS & Android (Unity 6.4, URP) · **Genre:** Lane Defense / Creature Collection / Loot Progression · **Model:** Free-to-play (fair F2P)
**Team:** Solo developer
**Repo:** `github.com/J-Koral/FablesUnite`

> **What changed since v1.0:** the skee-ball "Skee Vault" loot engine was replaced entirely by the **Growth Tree**; currencies were reworked (Tree Tokens / Gold / Raid Coins); a **Tower** mode was added; **Turf Wars** became the competition model; and the project moved from concept into a working client-side vertical slice. This version tags every system as **[Built]**, **[Planned]**, or **[Deferred]** so the current state of the game is readable at a glance.

---

## 0. Current Build State (read this first)

A quick, honest snapshot of what actually exists in the repo today versus what's designed but not yet built. Everything marked Planned/Deferred is still intended for the game — it just hasn't been reached yet.

**Built and working [Built]**
- Three-lane defense combat: deploy phase, auto-battle, element counters, recon, 1×/2×/4× speed, win/lose, win rewards, chapter-scaled enemy HP.
- The Growth Tree loot engine: weighted pick-one pulls, hold-to-spam (up to 15/burst), pity at 50, transparent per-spot odds on long-press, landing juice, fly-to-target reward motes, queued minigame spots.
- Fable progression: Level (XP), Stars (duplicate shards), Evolution (at max stars).
- Roster grid + Fable detail panel (feed / star / evolve).
- Currency HUD (Gold, Tree Tokens, Raid Coins), local JSON save/load.
- Scene shell for all modes: Home, Tree, Battle, Camp, Roster, Tower, TurfWars.
- One camp building (Token Grove) producing Tree Tokens.
- 5 Fable definitions (one per element), data-driven via ScriptableObjects.

**Designed, not yet built [Planned]**
- Multiplier toggle (1×–5×) and Fever Meter on the Growth Tree.
- Feed Rank (E→S) and Global Level progression axes.
- 5th rarity tier (Rainbow / Mythic) and rarity-as-evolution-stage coupling.
- Evolution Trials; star badge tiers (Bronze→Sun).
- Premium currency (Gems); Feed Kitchen and Decoration camp buildings.
- Full roster (target 60 evolution lines), evolution chains on existing Fables.

**Deferred to a later phase [Deferred]**
- Server-authoritative economy via Unity Gaming Services (currently local/client-side).
- Chapter campaign content (80-level chapters, mini-bosses, Dr. Vex story beats), staged onboarding ladder.
- Turf Wars as a live event; Tower mode content; raid missions.
- Loot games #2 and #3; Battle/Season Pass; 1v1 PvP; permanent clans.

---

## 1. Game Identity

### Elevator Pitch
A fantasy lane-defense game where players collect legendary heroes and magical creatures — called **Fables** — and unite them into squads to defend their realm against the chaos army of **Dr. Vex**, a comedic, over-the-top mad scientist who weaponizes natural disasters and builds monstrous soldiers to conquer the world.

### The Villain — Dr. Vex
A brilliant, unhinged scientist who believes the natural world is "inefficient" and has decided to improve it — by turning storms, lava, and tidal forces into obedient weapons. Dr. Eggman energy: loud, theatrical, always monologuing, never quite wins. His army mixes disaster-forged monsters with mechanical soldiers. Every chapter is a new scheme. He appears in short comic beats, taunts after losses, gloats when his bosses nearly win. Players love to hate him.

### The Fables
Each collectible is a legendary being from the realm's mythology. The roster mixes:
- **Hero Fables** — human archetypes: warriors, mages, rangers, druids, rogues
- **Creature Fables** — fantasy beasts: fire drakes, storm spirits, fae guardians, earth golems, tide serpents

Both share identical systems (elements, levels, stars, evolution). Squads can be all heroes, all creatures, or mixed — that mix is a key differentiator vs. competitors.

### Game Pillars
1. **Collect with purpose** — every Fable feels distinct, useful, and worth chasing
2. **Defend with strategy** — formation and positioning matter as much as raw power
3. **Progress feels rewarding** — clear paths forward, no dead ends or grinding walls
4. **Personality everywhere** — Dr. Vex, the Fables, and the world all have charm

### Tone
Cute but with edge. Bright colors, expressive characters, playful animation — but Vex's army genuinely escalates and stakes feel real. Early Clash of Clans meets Saturday-morning-cartoon villain.

### Audience
Dual-layer: casual players get fast 5–10 minute sessions and simple rules; mid-core players get squad-building depth, elemental strategy, competition, and a long progression ladder.

---

## 2. Core Gameplay Loops

**Micro loop (one session, 5–10 min):**
Open app → pull the Growth Tree → earn resources → fight a battle → win rewards → upgrade a Fable → repeat

**Daily loop:**
Claim camp production (Token Grove) → battles → Growth Tree pulls → upgrade Fables → check events / modes

**Long-term loop:**
Progress campaign chapters → unlock & evolve Fables → compete in Turf Wars / Tower → chase rare Fables and top-tier evolutions

**The engine:** the Growth Tree fuels every progression axis (levels, shards, tokens, raid coins, Fables). Battles and the camp feed tokens back into the Tree. Everything interlocks.

**No energy system.** Players battle freely. Pacing comes from Tree Token availability and campaign difficulty, not stamina gates.

---

## 3. The Growth Tree (Primary Loot Engine)

> **Replaces the v1.0 "Skee Vault" skee-ball mechanic entirely.** Same underlying philosophy — weighted, tunable, server-decidable odds — new metaphor built for portrait phones.

The player's primary loot engine and Fable-acquisition system. A tall **vertical tree** with reward **spots** on its branches. The player spends **Tree Tokens** to "pull," and each pull lights exactly one spot; that spot's prize is awarded.

### Core Principle [Built]
Each pull is a **weighted pick-one**: the game rolls across all spot weights and lights exactly one spot by its shown odds. Because one spot always lights, the player never whiffs — no separate "floor" rule is needed. The reward metaphor (a spot lighting up on the tree) is cosmetic; the outcome is decided by weighted RNG the instant the pull happens.

> Design note: today the roll runs client-side. When the economy is hardened (see §12), the roll moves server-side so odds and rewards can't be tampered with. The client code barely changes shape.

### Spot Layout & Reward Types [Built]
Spots are data (a `TreeConfig` asset holding a list of `TreeSpot`s), so tuning and adding spots never touches code. Each spot has a label, reward type, rarity color, amount, and relative weight. Reward types currently supported:

| Reward Type | Notes |
|---|---|
| Gold | Soft currency (building upgrades, store basics) |
| Tree Tokens | The pull currency itself — the Tree partly refills itself |
| Raid Coins | Competition currency (feeds Turf Wars) |
| Fable Shards | Duplicates toward a Fable's next star |
| Fable | A new creature (or shards if already owned) — the gacha payoff |
| XP | Grants XP toward Fable levels |
| Minigame | Queues a bonus minigame, played after the pull burst |

The **same item can sit on multiple spots** at different rarities (e.g. a common small-Raid-Coin spot and a rare big-Raid-Coin spot); the rarer hit flashes the rarer color. **Fables live on the rarest spots** (very low weight) — the Tree is the creature gacha.

### Pull Feel [Built]
- **Hold-to-spam:** a tap is one pull; press-and-hold streams pulls at a fixed interval, capped at **15 per hold**.
- **Transparent odds:** long-press any spot to see its label and exact % (weight ÷ total weight).
- **Landing juice:** the lit spot flashes and punch-scales in its rarity color; rarer hits trigger a haptic buzz.
- **Fly-to-target:** a burst of reward "motes" flies from the lit spot toward the on-screen destination for that reward (Gold → Camp, Tokens → top-bar counter, Raid Coins → Turf Wars, Fables/Shards → Roster). Teaches the player what they earned with no words.
- **Minigames don't interrupt:** a minigame spot hit mid-hold is queued and presented after the finger lifts, so a 15-pull burst stays uninterrupted.

### Pity [Built]
A **guaranteed Fable within 50 pulls** without one. The pity counter advances on every non-Fable pull and resets on any Fable. This protects unlucky players and makes the chase feel bounded.

### Ball → Token Economy [Built]
Tree Tokens are deliberately easy to gather: battle wins, the camp's Token Grove building, and the Tree itself (some spots grant tokens). Because access is generous, economy control lives in **spot weights and reward sizes**, not scarcity. Rare rewards stay rare via odds, not access. Running out of tokens is the natural, non-punishing monetization point.

### Multiplier Toggle [Planned]
Pre-pull toggle: **1× / 2× / 3× / 5×.** Consumes that many tokens; multiplies the landed reward by the same factor. Expected value is fair; variance (the thrill) increases. High-rollers opt in; cautious players ignore it. *(Carried over from v1.0's skee-ball screen; needs a home on the Tree UI.)*

### Fever Meter [Planned]
Every pull fills a meter regardless of outcome. When full → **Fever Round:** the next few pulls get boosted odds (Fable chance temporarily raised). Converts time-spent into guaranteed excitement and protects cold-streak players from frustration. *(Carried over from v1.0; not yet built.)*

### Identity vs. future loot games [Deferred]
The Growth Tree has the **highest base odds** of three planned loot mechanics — the reliable workhorse. Loot games #2 and #3 (designed later) trade lower odds for other thrills (bigger jackpots, exclusive rewards). Players will switch between them.

---

## 4. Combat System

Lane defense. Pure positioning strategy — all skill lives in the deployment phase. **[Built] for early game.**

### Structure
- **Lanes:** 3 today; scaling up to **6 lanes** late game **[Planned]**
- **Squad size:** 9 today (3 lanes × ~3); target ~18 late game as lanes scale **[Planned]**
- **Win condition:** pure defense — survive all waves; enemies must never breach the back line

### Battle Flow [Built]
1. **Recon** — a per-lane preview of incoming elements, so the player can read where the pressure is. *(Force-weight per lane — "this lane needs 4 defenders, that one needs 2" — is [Planned] tuning on top of the current element preview.)*
2. **Deploy** — place your squad up front by tapping a lane; no mid-battle reinforcements.
3. **Auto-battle** — Fables auto-attack the nearest enemy in their lane; enemies march toward the back line and stop to fight.
4. **Speed control** — 1× / 2× / 4× so players never wait to learn if their setup worked.

### Element Counter Ring [Built]
💧 Water → 🔥 Fire → 🌿 Grass → ⛰️ Ground → ⚡ Electric → 💧 Water
Each element beats one, loses to one, and is neutral to two. Tuning values in code: **strong ×1.5, weak ×0.75, neutral ×1.0.** Casual players learn it in one battle; optimizers min-max lane assignments.

### Difficulty [Built]
Enemy health scales with the player's `chapterLevel` (currently +12% per chapter). Winning a battle advances the chapter and grants rewards (currently +5 Tree Tokens, +50 Gold). Waves are authored as data (`WaveSpawner`), so new encounters don't require code.

---

## 5. Fable Progression System

Progression is designed as multiple axes, each fed by a different resource so they never feel redundant. Three axes are **built**; two more are **planned**.

| Axis | What it does | Source | Status |
|---|---|---|---|
| Level | Per-Fable stat boost as XP fills | XP (Tree, later feeding) | [Built] |
| Stars | Stat boost; gates evolution | Duplicate shards | [Built] |
| Evolution | Form change / new identity at max stars | Star cap + linked definition | [Built] |
| Feed Rank (E→S) | Extra per-Fable stat boost when a feeding bar fills | Feeding items (Feed Kitchen) | [Planned] |
| Global Level | Passive buff to ALL Fables automatically | Playing the loot games | [Planned] |

### Stats [Built]
Effective stats = base × (1 + perLevelGain × (level−1)) × (1 + perStarGain × (stars−1)). Defaults: +8%/level, +15%/star. Health and damage both scale; each Fable definition carries its own base stats, attack speed, and range.

### Rarity [Built → Planned extension]
Currently **4 tiers**: 🔵 Common → 🟣 Rare → 🟡 Epic → 🔴 Legendary. A **5th tier, 🌈 Rainbow (Mythic), is [Planned].** Also planned: coupling **rarity to evolution stage** (each evolution bumps the rarity color, top lines reaching Rainbow). Today rarity is a fixed field per Fable and evolution is a separate optional chain; the two will be linked in the full design.

> Design intent (unchanged): **kit > color.** A well-built Common should be able to out-perform a base Rare in the right lane. Meta diversity is a goal, not a bug.

### Stars — Display [Built → Planned polish]
Stars currently show as plain asterisks / "n / max." The **badge tiers (Bronze → Silver → Gold → Moon → Sun)** are [Planned] polish. Duplicate cost scales with star level.

### Evolution [Built → Planned gating]
At max stars, a Fable can evolve into a linked definition (resets to level 1 / 1 star with a new identity). **Evolution Trials** — combat/collection goals gating each evolution so it feels earned — are [Planned]. Existing Fable assets don't yet have their evolution chains authored.

### Roster [Planned]
Target **60 base evolution lines** shipped in waves as live content (each line = 3–5 forms; full roster ≈ 200+ forms). Today there are **5 Fable definitions**, one per element, used to prove the systems.

---

## 6. Campaign, Progression & Onboarding [Deferred]

Currently only `chapterLevel` and enemy scaling exist. The full structure below is designed for Phase 2.

### Chapter Structure
- ~80 levels per chapter
- Mini-boss every 10 levels (7 per chapter) — difficulty and learning checkpoints
- Chapter boss at level 80 — one of Dr. Vex's disaster creations
- Each chapter = a new Vex scheme, a new location/background theme
- Enemy mix: always mixed elements, weighted toward each chapter's featured element — keeps every battle a placement puzzle while giving chapters identity

### Story Format (light)
Two short, skippable comic-panel beats per chapter: scheme reveal (Vex monologue) at chapter start, and a defeat/escape tease at the chapter boss.

### Staged Unlock Ladder (design principle: never reveal two systems at once)
| Player reaches | Unlocks |
|---|---|
| Level 1 | Battle, 3 lanes, auto-battle, speed control |
| Level 2–3 | Element counters |
| Level 4 | Growth Tree |
| Level 6–8 | Feeding (Feed Rank E→S) |
| Level 10 | First mini-boss + Camp |
| Level 14–16 | Stars (duplicates) |
| Level 20+ | Evolution (first trial — the big "wow" moment) |
| Level 25–35 | 4th–5th lanes |
| End of Chapter 1 | Competition modes unlock |
| Mid-game | Loot games #2–#3; 6th lane; co-op |

---

## 7. The Camp (Home Base)

A buildable home base where every building feeds an existing system. Private (not destructible). **One building built; two planned.**

| Building | Produces | Feeds | Status |
|---|---|---|---|
| Token Grove | Tree Tokens over time | Growth Tree | [Built] |
| Feed Kitchen | Feeding items | Feed Rank (E→S) | [Planned] |
| Decorations | Personalization | Expression + monetization | [Planned] |

Production model: timer-based generation up to a storage cap; collect before full. Upgrading buildings (Gold) raises rate + cap — the primary soft-currency sink and a daily re-engagement hook. *(Current Token Grove uses a simple fixed batch on collect; real time-based generation is [Planned].)*

**Open decision:** decorations purely cosmetic vs. tiny adjacency bonuses. Decide during monetization tuning.

---

## 8. Competition, Events & Modes

**Design goal (non-negotiable):** no dying servers. Competition stays healthy at any population size, and every player gets access to genuinely even competition. All of this is **[Deferred]**; the mode scenes exist as shells today.

### The Three Modes (bottom-bar nav)
- **Battle** — the campaign / lane defense. [Built]
- **Tower** — a new mode added since v1.0 (endurance/climb style). Scene shell only; design TBD. [Planned]
- **Turf Wars** — the flagship competition event (below). Scene shell only. [Deferred]

### Turf Wars (competition model) [Deferred — concept locked]
A competitive, multi-team territory event where rival hero teams race to seize and hold Dr. Vex's network of towers. Whoever controls his towers the **longest** wins.
- **Teams:** four factions (Red / Blue / Yellow / Purple), ~80 players each *(size to test)*.
- **Groups:** small friend-squads (~4–5) inside a team who coordinate and earn group rewards.
- **Combat reuse:** defending a tower = a placed Fable formation; attacking = your Fables assault the garrison. Element ring + recon drive strategy.
- **Matchmaking:** assigns groups across teams to balance total power — the "spread power evenly" guarantee.
- **Currency:** powered by **Raid Coins**, already seeded as a Growth Tree reward.
- **Fairness:** score-based (works at any population), tiered rewards for everyone, additive-only (defenders never lose earned resources offline), and an equalized "fair-fight" variant where spend is flattened and squad-building skill decides.

### Raid Missions [Deferred]
Generated as a Growth Tree bonus — ties raiding into the loot loop. Attack an offline snapshot of another player's defense; win → rewards. Additive-only (defenders never lose resources; they earn passive rewards when a defense holds). Power-banded matchmaking.

### Deferred further
Generic 1v1 PvP and permanent clans/guilds — added post-launch only if population supports healthy queues.

---

## 9. Monetization & Economy

### Philosophy
Fair F2P. Spending accelerates progress and buys cosmetics — it never buys exclusive power, and equalized events guarantee a pure-skill arena regardless of spend. Protecting the competitive population is a revenue strategy, not just ethics.

### Acquisition Model (no gacha banners)
The **Growth Tree is the acquisition engine.** Fables and their shards come from Tree pulls; everything is earnable free, and spending only speeds it up.

### Currencies
| Currency | Type | Use | Status |
|---|---|---|---|
| Tree Tokens | Soft, abundant | Growth Tree pulls | [Built] |
| Gold | Soft | Building upgrades, store basics | [Built] |
| Raid Coins | Soft (competition) | Turf Wars / raids | [Built] |
| Gems | Premium (buy or slow-earn) | Store, convenience | [Planned] |
| Shards / XP | Progression items | Fable growth | [Built] |

> **Changed since v1.0:** the old Skee Balls / Gems / Capsules / Wishboxes / Feed currency set was reworked. Tree Tokens replace Skee Balls; Raid Coins are new; capsules/wishboxes are folded into Tree reward types; Gems (premium) are planned but not yet in the build.

### Revenue Streams [Planned]
Convenience (extra tokens, boosts, the multiplier), a rotating generic store (Fables/shards/cosmetics/gem bundles/decorations), optional rewarded ads (watch → free tokens; never forced), and a Battle/Season Pass (flagged for later evaluation).

### Trust Mechanics [Built]
- **Published odds** on every Tree spot (long-press to view).
- **Pity system** — guaranteed Fable within 50 pulls.

---

## 10. Art Direction

2D cute cartoon creature-collector (CoC-style traits, original characters):
- Chibi-ish proportions — rounded bodies, big expressive faces, readable silhouettes at mobile size
- Bold clean outlines + soft cel shading; bright, saturated, friendly palette by default
- Cozy decorative camp (Cookie Run Kingdom vibe)
- **"Edge" via contrast:** Dr. Vex's disaster zones and bosses use darker, moodier tones and sharper shapes against the cheerful baseline
- Touchstones: Cookie Run Kingdom (camp/cuteness), Pokémon (creature appeal/readability), plus the mad-scientist-disaster twist

**Hard rule:** match the general style, never specific characters. Every Fable design is original.

**Current state:** placeholder art and element-colored shapes; a real art pipeline is Phase 2 work. Art is the long pole and will be produced in waves.

---

## 11. Audio Direction

- **Music:** upbeat, playful, cheerful adventure; moodier variants in Vex disaster zones for contrast
- **SFX:** satisfying and juicy — weighty pull/land thunks, escalating reward chimes, big fanfare on a Fable hit. Loot-feel is a retention feature.
- **Voice:** charming gibberish/grunts for Fables and Dr. Vex — cheap, expressive, no VO budget required

---

## 12. Technical Architecture

### Current architecture [Built] — client-side vertical slice
- **Engine:** Unity 6.4, Universal Render Pipeline, portrait, Input System package.
- **Persistence layer:** a `GameData` singleton (`DontDestroyOnLoad`) holds `PlayerData` (currencies, roster, chapter, pity); a `SceneLoader` singleton handles fade transitions between scenes. Both survive scene changes.
- **Data-driven content:** Fables, the Fable database, and the Tree config are **ScriptableObject assets** — new content is a new asset, not new code. Save data stores **id strings**, not asset references, so it serializes cleanly.
- **Save:** local JSON to `persistentDataPath`, written on change and on pause/quit.
- **Navigation:** each mode is its own scene (Home, Tree, Battle, Camp, Roster, Tower, TurfWars) loaded via the `SceneLoader`; the Fable detail view is a panel within Roster. A persistent bottom bar carries Tree (home) / Roster / Camp; Battle / Tower / Turf Wars are full modes.

### Target architecture [Deferred] — server-authoritative economy
The stated core rule: anything affecting economy or competition should eventually be decided on the **server**, never the client — Tree rolls, reward grants, raid outcomes, event scores, currency balances, purchases.

**Recommended path:** Unity Gaming Services (UGS) — Cloud Code (RNG rolls, reward grants, raid resolution, event scoring, purchase validation), Cloud Save (replaces local JSON, follows the account across devices), Remote Config (Tree weights, pity, reward sizes, difficulty — tunable live without app updates), Leaderboards (event brackets/rankings). The client's `TreeController.Pull()` becomes "ask the server"; `SaveSystem` becomes Cloud Save — same loop, now cheat-proof.

> This is a **hardening** step, taken once the loop is proven fun — not a prerequisite for proving it.

---

## 13. Development Roadmap

Principle: prove fun before building scale. Each phase ends with a go/grow/scope-down decision.

- **Phase 0 — Prove the Fun [Done].** Grey-box lane defense: 3 lanes, element counters, auto-battle, win/lose.
- **Phase 1 — Vertical Slice [In progress].** Growth Tree, data-driven Fables with level/star/evolve, Home hub + roster + currency HUD, minimal camp, save/load, first-pass art, deploy your real roster. Deliverable: does the **battle → tree → upgrade → harder battle** loop hold your own attention for a week?
- **Phase 2 — Content & Systems [Planned].** Chapter 1 (80 levels, mini-bosses, Vex beats), staged onboarding, real art pipeline, Feed Kitchen + Feed Rank, more Fable lines, Rainbow rarity, evolution trials, multiplier + Fever Meter.
- **Phase 3 — Live-Ops & Competition [Planned].** Turf Wars event framework, Tower mode, raids, leaderboards, equalized events, server-authority via UGS + Remote Config scheduling.
- **Phase 4 — Soft Launch [Planned].** One market; measure D1/D7/D30, session length, conversion; tune odds/difficulty/economy hard.
- **Phase 5 — Global Launch & Live Content [Planned].** Steady Fable-line and chapter cadence; loot games #2–#3; Battle Pass evaluation; 1v1 PvP and permanent clans if population supports them.

---

## 14. Open Decisions (Pinned)

| # | Decision | Revisit when |
|---|---|---|
| 1 | Where the multiplier toggle + Fever Meter live on the Tree UI | Phase 2 Tree polish |
| 2 | Tower mode design (rules, rewards, progression) | Phase 3 |
| 3 | Loot games #2 and #3 design | Phase 3+ |
| 4 | Decorations: pure cosmetic vs. tiny bonuses | Monetization tuning |
| 5 | Battle/Season Pass | Post-soft-launch |
| 6 | Generic 1v1 PvP | Post-launch, population-dependent |
| 7 | Permanent clans/guilds | Post-launch |
| 8 | 5-evolution Fable lines | Live content |
| 9 | Exact stat formulas, damage multipliers, spot reward contents | Balancing passes (Phases 1–4) |
| 10 | How rarity couples to evolution stage | Phase 2 progression pass |

---

*Fables Unite — GDD v2.0. Every system feeds another: the Tree fuels progression, progression wins battles, battles fund the Tree, the camp ties it together, and fair competition keeps the world alive.*
