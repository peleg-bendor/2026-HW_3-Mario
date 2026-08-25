# HW-3 Plan — `2026-HW_3-Mario` (Exercise 3)

Shared working notes for Exercise 3, modeled on `2026-HW_2-Mario/HW_2_PLAN.md`. The assistant may
edit this file directly as we go (it's not game code, just shared notes).

The project starts from a copy of `2026-HW_2-Mario`'s finished `Assets` — everything Exercise 2 was
graded on, plus the logging system, the conventions pass and the finished level. That project is
closed; see its own `HW_2_PLAN.md` for how its stages went.

**Due 4.9.2026.**

## Status Legend

- `[ ]` not started
- `[~]` in progress
- `[x]` done AND confirmed working in-editor

## Git Workflow Reminder

- After any step that leaves the project in a working state, consider committing (small, working
  commits > one giant commit).
- After a whole stage is finished and confirmed working, push.

## What Exercise 3 asks for

One new weapon — a laser — that Mario can only fire after collecting a power-up, built through four
named design patterns. The exercise numbers the patterns 1 to 4, then lists the power-up, the firing
rule and the integration separately.

| Exercise item      | What it names                     | What it asks for                                                                              |
| ------------------ | --------------------------------- | --------------------------------------------------------------------------------------------- |
| 1 — Builder        | `LaserBuilder`, `LaserDirector`   | build the projectile in steps (speed, animation, size, damage)                                |
| 2 — Factory        | `LaserFactory`                    | return a ready projectile using the builder, so callers don't know the construction           |
| 3 — Object Pooling | `LaserPoolManager`                | fired takes from the pool, hit or expired returns to it                                       |
| 4 — Template       | `BaseProjectile` with `Fire()`    | the laser inherits it and fires straight up only                                              |
| Power-up           | `LaserPowerUp`                    | modelled on `FireFlowerPowerUp`                                                               |
| Firing             | —                                 | straight up; through enemies or dying on hit is our choice; the logic lives in the projectile |
| Integration        | `WeaponsHandler`, `PlayerPowerUp` | no laser without the power-up; log fired / taken / hit / returned                             |

Then: record each item working, show and explain the code per item, and play the game at the end —
the same submission shape as Exercises 1 and 2.

## Design vocabulary

The course builds a vocabulary and this project is expected to speak it. Proposals get judged against
this rather than against generic best practice, and the video is where it gets said out loud.

- **SOLID** (Lessons 2-4). Already load-bearing: `IWeapon`/`IUseableWeapon`/`IReloadWeapon`,
  `IPowerUp`, `IEnemy`, and `TilePrefabMap` replacing the lesson's hardcoded `switch`.
- **MVC** (Lesson 7). The health system is built this way; Stage 0.5 converts the coin counter.
- **Builder and Object Pooling** (Lesson 9), **Factory and Template Method** (Lesson 10). What this
  exercise is actually about.
- **Strategy and Clean Architecture** (Lesson 11). No exercise item asks for either, and neither is
  being built for its own sake. Clean Architecture still matters here, because its central rule is
  one this codebase already half-follows: keep logic out of MonoBehaviours. `HealthModel` is plain C#
  with no Unity types precisely so it can be constructed without a scene, which is the Entities layer
  in everything but name. Exercise 3's builder, director and factory are the same shape - none of
  them needs to be a MonoBehaviour, and the pool is the only piece that does. Worth naming when a
  design choice lands on that line.

None of this means reaching for a pattern that isn't needed. This project's own rule - no abstraction
before a second real use case - outranks a pattern's name every time, and Exercise 2 declined
invented work twice on exactly that basis.

## Stage Order

**The exercise's numbering is a deliverable order, not a build order, and this is the first exercise
where the two genuinely disagree.** A builder with nothing to build is meaningless, so the projectile
and its base class have to exist before item 1 can be written; the factory needs the builder; the
pool needs the factory. Stages below are sequenced by dependency, each labelled with the exercise
item it satisfies. The video presents them in the exercise's own order — which is exactly what
Exercise 2's video already did, showing items 1, 2, 3, 4, 8, 5, 6, 7, 9 against a different build
order. See `HW_2_PLAN.md` Stage 10.

| Stage                                            | Exercise item                        |
| ------------------------------------------------ | ------------------------------------ |
| 1 — `BaseProjectile` and `ProjectileLaser`       | item 4, Template                     |
| 2 — `LaserBuilder` and `LaserDirector`           | item 1, Builder                      |
| 3 — `LaserFactory`                               | item 2, Factory                      |
| 4 — `LaserPoolManager`                           | item 3, Pooling                      |
| 5 — The weapon, the power-up and the integration | power-up, firing, integration        |
| 6 — Boomerang                                    | nothing; Peleg's addition            |
| 7 — Full playthrough                             | nothing; regression before the video |
| 8 — Video script                                 | the submission                       |

Stage 0 gets the project versioned and fixes what didn't travel in the `Assets` copy. Stage 0.5 is
inherited-code cleanup carried over from Exercise 2's parked list.

## Tile Roster

Two new placeable tiles, both pickups. The laser *ray* is not a tile — it is spawned at runtime and
never placed in a level, so its prefab gets no `Sprite_` prefix, the same as `Fireball.prefab` and
`Axe.prefab` already do. Same for the thrown boomerang, which is a separate prefab from the boomerang
pickup exactly the way `Axe.prefab` is separate from `Sprite_Axe.prefab`.

| `TilePrefabMap` id | Unity prefab              | Stage |
| ------------------ | ------------------------- | ----- |
| 1-16               | unchanged from Exercise 2 |       |
| 18                 | `Sprite_LaserGun`         | 5     |
| 19                 | `Sprite_Boomerang`        | 6     |

Runtime-only prefabs, no tile id and no `Sprite_` prefix: `LaserRay.prefab` (Stage 1),
`Boomerang.prefab` (Stage 6).

**The crate collision, resolved in Stage 0.** `MarioTiles.tsx` held the crate from Exercise 2's video
at Tiled id 16. Removing it through Tiled's own tileset panel didn't free that id back up — Tiled
appends past the highest existing id rather than backfilling a gap — so the laser gun landed at
tileset id 17 and the boomerang at 18, one higher each than this plan originally reserved. The Tile
Roster above reflects the actual ids.

### Stage 0 — Repo and project setup `[x]`

Getting `2026-HW_3-Mario` from "a copied folder that runs" to "a versioned project with a clean
starting commit."

#### Step 1 — Project created from Exercise 2's `Assets` `[x]`

Done by Peleg before this plan existed: `Assets` copied wholesale from `2026-HW_2-Mario`, Build
Settings checked, the `Tools > Level`, `Tools > Tile Placer` and `Tools > Logs` windows re-assigned
their references, and the game played through. Confirmed working.

Also already done: `Sprite_LaserGun.png`, `Sprite_LaserRay.png` and `Sprite_Boomerang.png` imported
into `Assets/Sprites/` at 48px, and `Sprite_LaserGun.png` and `Sprite_Boomerang.png` copied into
`Tiles01/Tiles/`. The ray is deliberately absent from `Tiles/`, which is correct — it is never a tile.

#### Step 2 — The three root files that didn't travel `[x]`

`CONVENTIONS.md`, `README.md` and `.gitignore` all sit at the *project root*, not inside `Assets`, so
copying `Assets` left every one of them behind. Copied by Peleg, then corrected:

- `.gitignore` — unchanged. Checked line by line; nothing in it names Exercise 2.
- `CONVENTIONS.md` — three fixes. Its claim that it "travels to the next exercise along with
  `Assets`" was false and had just cost three files, so it now says the opposite and names all four
  that have to be copied by hand. Its two references to `HW_2_PLAN.md` became references to "the
  current exercise's plan file", so that goes stale once rather than every exercise. No rule changed:
  they held across all of Exercise 2 and there is no evidence against any of them.
- `README.md` — rewritten. New starting point, a what-this-exercise-adds section naming the four
  patterns, and a short logging section Exercise 2's README never had. The seven editor-tooling
  departures, the level section and the submission note carry over unchanged.

Also updated, outside this project: the repo-root `CLAUDE.md`, which loads at the start of every
session and still pointed at Exercise 2 for every path. It now points here, and carries a Design
vocabulary section mirroring the one above.

#### Step 3 — `.gitattributes` and the line endings `[x]`

Exercise 2's logging conversion wrote LF into a CRLF project and left the repo holding both. That
split travelled here intact: **46 of the 63 scripts are LF and 17 are CRLF**, confirmed by reading the
files rather than assumed.

Nothing is broken by this and nothing will be. It matters for one reason: any tool that rewrites a
whole file in the other convention turns a two-line change into a whole-file diff, which is exactly
what happened twice while Exercise 2's plan was being edited. The fix is a `.gitattributes` telling
git to normalize on commit, not another sweep through the files.

#### Step 4 — The obsolete API warning `[x]`

`LogsWindow.cs:24` calls `FindFirstObjectByType<T>()`, which Unity has deprecated in favour of
`FindAnyObjectByType`, on the grounds that the "first" version depends on instance-id ordering. The
window wants *the* `LogSettings` in the open scene and there is only ever one, so ordering is
irrelevant here and the rename is a one-word edit with no behaviour change.

Worth doing in Stage 0 rather than living with it: the video puts the Console on screen, and a
standing warning there invites a question that costs more to answer than the fix costs to make.

#### Step 5 — A clean start in Tiled `[x]`

Per Peleg: rather than carry Exercise 2's leftovers, get Tiled correct once and export from it, so
the level file, the map and the scene all agree before any feature work starts.

Three things are wrong with `Tiles01/` as it stands, and one of them is dangerous:

- **The export target still points at Exercise 2.** `Level01.tmx` carries
  `<export target="../2026-HW_2-Mario/Assets/Levels/Level01.txt">`. Pressing Export in Tiled today
  overwrites the submitted Exercise 2 level file. Repoint it *first*, before anything else in this
  step and before opening the map for any other reason.
- **The crate is still in the tileset**, at Tiled id 16, left over from Exercise 2's video. Its Unity
  prefab and its mapping row were deleted after the take, but nothing removed the tileset entry,
  because `Tiles01/` is in no git repo and nothing read it again. It also still sits in the map at
  (12, 3). Left alone it takes the id the laser gun wants.
- **`Level01.tmx` is two cells behind `Level01.txt`**: that crate at (12, 3), and a coin at (24, 4)
  the Tile Placer added on camera which never travelled back.

**What actually happened, in order:** the export target got repointed first, as planned. The crate
came out of the tileset and off the map cleanly. Adding the gun through `Tileset > Add Tiles` landed
it at id 17 rather than the hoped-for 16 (Tiled appends past the highest id rather than reusing a
freed one), and that same pass also stamped it onto the map at (1, 16), overwriting an axe pickup
that was already there — not something this step called for, caught only after an Export had already
carried it into `Assets/Levels/Level01.txt`. Rebuilding the level from that file before the mistake
was caught baked the loss in a second way: `LevelWindow` skips any tile id it has no mapping for yet,
so the cell built as empty rather than as axe or gun, and Save Level then wrote that emptiness back
out. Per Peleg: leave it. The cell resolves into an actual laser gun once `TilePrefabMap` gets a row
for id 18 and it gets placed properly with the Tile Placer, so nothing further is owed here. The
boomerang went in afterward without repeating the mistake, landing at id 18 with no map placement.

One other loss from the same window, unrelated to the gun: `Level01.tmx`'s missing coin (the one Tile
Placer had added on camera, never present in Tiled) never got carried back into `Level01.txt` before
the Export overwrote it, so that coin is also gone from the level. Per Peleg: also fine to leave.

That leaves Tiled holding the current level with both new tile types registered, `TilePrefabMap` ids
18 and 19 free for them, and nothing left over from the previous exercise anywhere in the pipeline —
at the cost of one axe and one coin pickup that Exercise 2's copy had and this level no longer does.

#### Step 6 — Git `[x]`

`git init` and an initial commit, so history starts from a working, cleaned-up state. The remote is
the same open question Exercise 2 had; see that plan's Stage 0 Step 3 for how it was settled.

### Stage 0.5 — Inherited-code cleanup `[x]`

Carried over from Exercise 2's parked Stage 11, minus everything dropped there.

#### Step 1 — Design discussion `[x]`

#### Step 2 — The coin counter as MVC `[x]`

`SC_CoinsManager` is the last class in the project that both owns a count and draws it. Health, axes
and the selected weapon all split those jobs already, so it is the odd one out regardless of MVC, and
Exercise 2 built and tested the exact shape it needs — `IHealthModel`, `HealthModel`, `HealthView`,
`HealthController`.

Two things make this worth doing now rather than parking it again. It is a copy of a proven shape
rather than a design problem. And Lesson 11's Clean Architecture material uses *coins specifically*
as its worked example — `PlayerStats`, `CoinCollectionService`, `CoinsController`, `CoinUIController`
— so the one piece of inherited code still doing two jobs is also the one the newest lesson happens
to be about.

No exercise item asks for it. Whether it gets any video time is a Stage 8 decision, not this one.

**Dropped, per Peleg:** the fade on the disappearing tile. Purely cosmetic, and the tile has already
been demonstrated working on camera.

**Built smaller than `IHealthModel`, not a straight copy of it.** `IHealthModel.Gain()`/`Lose()`
return `bool` because health has real ignored cases - capped at max, floored at zero. Coins have
neither: every pickup increments, unconditionally, forever. `ICoinsModel.Gain()` returns `void`
rather than carrying a return value that would never be `false`.

**`CoinsView` is its own GameObject under `Scripts`, not a component on `Txt_Coins`.** The exact
mistake Exercise 2's Stage 2 caught and corrected for `HealthView` - `CONVENTIONS.md`'s hierarchy
rule puts logic-only manager objects in `Scripts` and GUI objects under `Canvas`, and a View script
is logic even when what it touches is a label.

**Confirmed working** via `GameLog.txt`: twelve straight `Coin collected: <name>` / `Coin collected -
N total` pairs, counting cleanly through a fire flower, two speed boosts, and a hazard hit and
respawn with no reset or duplicate.

### Stage 1 — `BaseProjectile` and `ProjectileLaser` `[ ]`

Exercise item 4, the Template pattern. Built first, because everything else in the exercise builds,
makes or pools the thing this stage creates.

#### Step 1 — Design discussion `[ ]`

What this stage has to settle, none of it decided yet:

- **What the template method actually is.** Template Method proper is a concrete method on the base
  that calls a fixed sequence of steps, some of them abstract, on itself — the lecture's
  `EnemyAI.ExecuteBehavior()` calling `Patrol()`, `DetectPlayer()`, `Attack()`. A `Fire()` that is
  itself abstract would not be Template Method at all, just an interface with extra steps. So `Fire()`
  has to *be* the template: reset state, apply motion, start the lifetime clock, with the motion step
  abstract and the laser's version sending it straight up.
- **Which projectiles inherit it.** Settled: the fireball does, the axe does not, the garlic is
  open, and Stage 6's boomerang does. See the Decisions Log for the reasoning; the consequence for
  this stage is that `BaseProjectile` is designed against the fireball and the laser together rather
  than against the laser alone, so every abstract step has two real implementations from the day it
  is written.
- **What the steps are, given that.** The four projectiles already share a shape: set facing, apply
  one launch impulse, run a lifetime, and end on either a target hit or terrain. What varies is the
  launch (sideways by facing, or straight up), what counts as a target, whether terrain stops it, and
  how it ends - `Destroy` for an ordinary projectile, `SetActive(false)` for a pooled one. That last
  one is what stops `Expire()` being a step that only ever does one thing.
- **The cost of retrofitting.** `ProjectileFireball.Attack(float)` becomes the base's `Fire(float)`,
  which changes one call site in `FireballWeapon`. Both are code two exercises have already graded,
  so it wants its own commit and a regression check of its own: a fireball into an enemy, a fireball
  into a wall, and one left to expire.
- **Whether the axe and the garlic join.** Probably not. The axe lands, freezes and is recollectable;
  the garlic is enemy-fired. Both would bend the template rather than fit it.

#### Step 2 — `BaseProjectile` `[ ]`

#### Step 3 — `ProjectileLaser` `[ ]`

Straight up, and the firing rule from the exercise lives here rather than in the weapon. Peleg's
reading, to be confirmed in Step 1: it passes through tiles and pickups, stops at enemies, and expires
after about 3 seconds either way. Both endings return it to the pool rather than destroying it, which
is Stage 4's requirement reaching back into this class.

#### Step 4 — Sprite, prefab and playtest `[ ]`

`LaserRay.prefab`, with no `Sprite_` prefix, since nothing ever places it in a level.

### Stage 2 — `LaserBuilder` and `LaserDirector` `[ ]`

Exercise item 1, the Builder pattern.

#### Step 1 — Design discussion `[ ]`

The open question: **the exercise names speed, animation, size and damage as the things the builder
sets, and this game has only two of them.** No projectile has an animation system, and there is no
damage system at all — `IEnemy.Kill()` destroys an enemy outright and nothing anywhere holds hit
points. Inventing both to satisfy a list would be building for an examiner rather than a need, which
is the trade this project has already declined twice: Exercise 2's all-tiles image, and the proposal
to add code to `LevelWindow` so item 5 would look like work.

The likely answer, to be settled: build speed, lifetime and size; say on camera that damage and
animation describe systems this game does not have; and let the sentence do the work the invented code
would have done badly. Same call and the same reasoning as Exercise 2's missing PNG.

#### Step 2 — `ILaserBuilder` and `LaserBuilder` `[ ]`

#### Step 3 — `LaserDirector` `[ ]`

### Stage 3 — `LaserFactory` `[ ]`

Exercise item 2, the Factory pattern. Returns a ready laser using the builder, so the pool never
learns the construction steps.

Worth knowing before the discussion: the lesson's own project does **not** do this. Lesson 9's
`FireballPoolSystem` holds the builder and director itself and calls them directly, with no factory
anywhere; Lesson 10 adds a Factory for enemies instead, unconnected to the pool. Exercise 3 asks for
the two to be stacked, which is a layering the course material never demonstrates together.

#### Step 1 — Design discussion `[ ]`

#### Step 2 — `LaserFactory` `[ ]`

### Stage 4 — `LaserPoolManager` `[ ]`

Exercise item 3, Object Pooling.

#### Step 1 — Design discussion `[ ]`

Two things already known to need deciding:

- **How the weapon reaches the pool.** Settled: a public static `Instance`, matching Lesson 9's
  `FireballPoolSystem`, plus the null guard that version lacks. An Inspector reference was never
  available - `LaserWeapon` lives on Mario, Mario is a tile the level builder places, and building
  deletes and recreates every child of `World`, so a prefab cannot hold a scene reference at all.
  See the Decisions Log for why the singleton beat the `FindAnyObjectByType` alternative.
- **What happens when every laser is in flight.** The lesson's pool returns `null` and its weapon
  silently does nothing — a sixth shot produces no laser and no feedback at all. This project's
  logging rules already have an answer for that shape: `<action> ignored - <reason>`. A cap is a
  consequence of a fixed pool rather than the point of one, but it is the part a player actually
  feels, so it should say so.

#### Step 2 — `LaserPoolManager` `[ ]`

#### Step 3 — Pool size and playtest `[ ]`

### Stage 5 — The weapon, the power-up and the integration `[ ]`

The exercise's remaining three sections: the `LaserPowerUp` pickup, the firing rule, and wiring the
whole thing into `WeaponsHandler` so Mario cannot fire a laser he hasn't earned.

#### Step 1 — Design discussion `[ ]`

**One problem is already known and has to be solved here.** `FireFlowerPowerUp` unlocks the fireball
with `player.GetComponentInChildren<IUseableWeapon>()` — the *first* one it finds. That works today
because the fireball is the only `IUseableWeapon` on Mario. A laser that is also locked until a
power-up is also an `IUseableWeapon`, and the moment both exist, each power-up unlocks whichever
component Unity happens to return first. The fire flower could equip the laser, or the laser pickup
the fireball, and nothing would report anything wrong.

This is a real bug that Exercise 3 introduces into working code, not a hypothetical, and it has to be
fixed in the same stage that creates the second implementer. The options are worth arguing properly
in the discussion — a typed lookup, a per-weapon identifier, or splitting the interface — because
whichever is chosen changes a class two exercises have already been graded on.

#### Step 2 — `LaserWeapon` `[ ]`

#### Step 3 — `LaserPowerUp` and `LaserGunController` `[ ]`

The pickup pair follows the shape every pickup in this project already uses: a controller that detects
Mario and hands over an `IPowerUp`, and a power-up that applies the effect without knowing who
collected it.

#### Step 4 — Registration and the tile `[ ]`

`PlayerWeaponsSetup` gains the laser. `TilePrefabMap` gains row 18 for `Sprite_LaserGun`. Q cycles
axe, fireball, laser — with the laser skipped until it is unlocked, which `IsAvailable()` already
handles for free.

#### Step 5 — Logging `[ ]`

The exercise asks for fired / taken from pool / hit / returned. This project logs through `GameLog`
rather than calling `Debug.Log` directly, which satisfies the intent and is worth one sentence on
camera rather than left looking like a missed requirement.

### Stage 6 — Boomerang `[ ]`

Peleg's addition. No exercise item asks for it.

A thrown weapon that flies sideways like the axe, turns around after about 3 seconds, and returns to
the point it was thrown from — the point, not Mario. If it isn't caught by the time it gets back, it
disappears, which is the whole difference from the axe: an axe waits on the ground to be reclaimed, a
boomerang is lost if missed.

Worth having beyond the fun of it: it is a genuine second or third implementer for Stage 1's
`BaseProjectile`, which is what keeps that base class from being an abstraction built for one caller.

#### Step 1 — Design discussion `[ ]`

#### Step 2 — Sprite, prefabs and behavior `[ ]`

Two prefabs, matching the axe exactly: `Sprite_Boomerang.prefab` for the pickup that sits in the
level, and `Boomerang.prefab` for the thing that gets thrown.

#### Step 3 — `BoomerangWeapon` `[ ]`

Almost certainly `IReloadWeapon` rather than `IUseableWeapon`, since a boomerang that can be lost is a
finite stock — the same reading that made the axe an `IReloadWeapon`.

#### Step 4 — Tile id and playtest `[ ]`

### Stage 7 — Full playthrough `[ ]`

Every item in one session, plus everything carried over from Exercises 1 and 2. Last stage before the
video, so it also covers whatever Stages 0 and 0.5 changed in code that was already working.

### Stage 8 — Video script `[ ]`

`HW_3-Script.md`, modeled on `HW_2-Script.md` — spoken lines in block quotes, stage directions outside
them, each requirement called out in Hebrew, one take per part, and every part written to a word count
rather than an estimate.

The measured pace to budget against, from two recorded videos: **about 160 spoken words a minute where
the camera is on a file, and about 90 where it is on the game.** Exercise 2's script came to 1888
words and recorded at 16:07 against a 16:55 estimate.

No length limit exists. Neither exercise text nor any course material sets one; Exercise 1's 15:00 was
a target Peleg set for himself, and Exercise 2 ran past it deliberately.

## Notes / Decisions Log

_(append entries here as we make design decisions.)_

- Stages are sequenced by dependency and labelled with the exercise item each satisfies, rather than
  numbered to match the exercise directly. This is the first exercise where the two orders disagree:
  the Builder is item 1 but cannot be written before the projectile it builds exists, and that
  projectile's base class is item 4. Exercise 2's video already established that build order and
  presentation order need not match, so the plan builds in one order and the video presents in the
  other.
- Lessons 8 and 11 are covered by the course and asked for by nothing. Exercise 3 names Builder,
  Factory, Pooling and Template only — no reflection, no DLL, no Strategy, no Clean Architecture.
  Exercise 2's plan parked reflection and the DLL on the grounds that "if Exercise 3 wants it, it
  belongs there"; it does not, so they are dropped rather than parked a second time. Lesson 11 does
  leave one mark: its Clean Architecture worked example is a coin counter, which is part of why
  Stage 0.5 converts coins rather than leaving them alone.
- The laser ray and the thrown boomerang get no `Sprite_` prefix on their prefabs, since that prefix
  marks things placed in a level and both are spawned at runtime. The pickups that *are* placed —
  `Sprite_LaserGun` and `Sprite_Boomerang` — keep it. Same split `Axe.prefab` and `Sprite_Axe.prefab`
  already have.
- The fade on the disappearing tile is dropped outright rather than parked a second time, per Peleg:
  purely cosmetic, and the tile has already been demonstrated working on camera.
- `BaseProjectile` is inherited by `ProjectileFireball` as well as the laser, from the day it is
  written. Raised by Peleg, against an earlier call to leave the fireball alone, and he is right:
  read side by side, the fireball and the garlic are nearly the same class already - set facing, one
  sideways impulse, destroy on a lifetime, and a trigger handler that ends the projectile on a target
  hit or on `SC_Floor`. The only real difference between those two is what counts as a target. With
  the fireball in, every abstract step has two genuinely different implementations rather than one,
  and `Expire()` in particular means `Destroy` for an ordinary projectile and `SetActive(false)` for
  a pooled one - which is the whole reason the pool works at all. The earlier argument against was
  that retrofitting means editing graded code, and it overstated the risk by comparing it to
  Exercise 2's logging rewrite: that touched 75 call sites across 37 files, this touches one file and
  one call site.
- The axe stays out of that hierarchy, on shape rather than caution. It arcs under gravity with two
  speed components, fades in `Update`, lands and hard-freezes, tracks the collider it froze against,
  resumes falling when that support moves out, and is recollectable on touch - and it works through
  `OnCollisionEnter2D` rather than triggers. Every one of those is a step the other projectiles have
  no use for, so including it would bend the template around its single most unusual member.
- The garlic is left open for Stage 1's discussion. It fits mechanically - it is the fireball with a
  different target test - but it is enemy-fired rather than one of Mario's weapons, and including it
  means retrofitting a second piece of graded code for tidiness rather than need.
- The laser pool is reached through a public static `Instance`, the shape Lesson 9's
  `FireballPoolSystem` uses, with the null guard that version lacks. `FindAnyObjectByType` in `Awake`
  was the alternative and is equally correct - `HealthController` already answers the same problem
  that way with a tag lookup. The singleton won for one reason: the video shows this code beside the
  lesson's, and departing from what the course demonstrated needs a reason this design does not have.
- `.gitattributes` goes in **before** `git init`, not after. Adding it later means a one-time
  renormalization commit touching every CRLF file; adding it first means the initial commit simply
  stores everything normalized and the 46/17 split never existed in this repo's history. This is the
  cheapest the fix will ever be, which is the argument for doing it now rather than parking it a
  third time.
- Tiled does not reuse a freed tile id - deleting the crate at id 16 didn't hand that id to the next
  tile added, it just appended past the highest one left. The laser gun and boomerang landed at ids
  17 and 18 instead of the 16 and 17 this plan originally reserved, which shifts every `TilePrefabMap`
  row this stage's Tile Roster promised by one. Updated in place rather than fought - chasing a
  specific id through a tool that doesn't guarantee one costs more than a table edit.
- One axe and one coin pickup are gone from `Level01.txt` and won't be coming back on their own, per
  Peleg. The axe was overwritten by a misplaced gun tile during Step 5's Tiled work, then built as
  empty (not axe, not gun) because `LevelWindow` skips ids `TilePrefabMap` doesn't know yet, and that
  emptiness got saved back out before it was caught. The coin was a Tile Placer addition that had
  never made it into `Level01.tmx`, lost the same way when Tiled's Export overwrote the file. Both
  are accepted losses rather than restorations - the gun cell resolves itself once `TilePrefabMap`
  gets a row for id 18 and the Tile Placer puts a real gun there, and the coin isn't worth a special
  trip back through Tiled for one cell.
- Stage 0.5's `ICoinsModel` is not a copy of `IHealthModel` - it's smaller by design. Health's
  `Gain()`/`Lose()` return `bool` because there are real ignored cases, capped at max and floored at
  zero. Coins have neither, so `Gain()` returns nothing rather than a value that could never be
  `false`. `CoinsView` also went on its own GameObject under `Scripts` rather than on `Txt_Coins`
  directly - the same placement Exercise 2's Stage 2 got wrong once already for `HealthView` and had
  to correct.