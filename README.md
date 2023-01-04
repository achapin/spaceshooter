# SpaceShooter

The goal for this project is to make a cooperative-multiplayer focused space shooter game in a vein similar to the old Rogue Squadron or Star Fox games.

## Wants list
The game should be
- Cooperative multiplayer up to 6 players
- Co-op focused, with specific features that make the game more fun to play as a team
- Level-based
- Extendable! Ships and levels should be addable easily by players

## Levels
Levels should be self-contained units, with different objectives that the players need to achieve in order to succeed. They should follow a "free world" format. For the time being, despite being a "space" shooter, I forsee most of the levels taking place on the surface of planets, moons, etc, to give the players a sense of orientation via the ground.

### Objectives
This is a short list of some of the objectives I would expect players might have to fulfill in order to progress through the level.
- Survive: A bunch of enemies spawn, players have to defeat all of them (or at least some percentage of them) before moving on
- Destroy: There's an enemy building/ship/whatever, and it needs to get blown up. Probably on a timer or something
- Protect: There's a friendly building/ship/whatever, and it needs to not get blown up.

## Ships
Ships should have a few manageable stats, and some tweaks that can be applied to those stats. They can also have different loadouts which can be preselected before starting a level.
Some ships should be multi-player. One pilot flies the ship, and other players can be gunners, bomb experts(?), or copilots.

## Flight
I'm really torn on whether to use a more flight-sim style flight setup where the player can control roll/pitch/yaw on their own, or if the more cinematic style used by most of my references is the way to go. The former will be easier to develop as it doesn't require complicated camera controls. However, for the style of gameplay I'm going for I think it might be too mentally taxing for the player and won't be as fun.

### Flight Systems
I do want the player to have to manage some basic flight systems. I like the Weapons/Engine/Shields setup used by Squadrons and Elite Dangerous, so I think I'm going to steal that approach wholesale. Players will have a set amound of energy "pips" that the ship has access to, and they can dynamically shift between a balanced power setup to favoring one system over the others in order to quickly charge it or gain a bonus.

### Engines
Ships will have a top speed that they can reach at each level of engine power. They will also have manuverability limits that change based on their current velocity. I _think_ that it will be fine to come to a full stop, instead of enforcing forward motion at all times, but that's not a decision I've settled on yet.

Like in Squadrons, I think it will be a nice gameplay element to have a boost, though unlike Squadrons I think the boost will constantly accumulate over time, but charge faster/last longer at higher power levels. I do _not_ intend to add any form of ship drifting like Squadrons, though.

### Weapons
All weapons will consume weapon power when fired, and if the weapons systems don't have power then they won't fire/acquire target locks, etc. Having an overcharged weapons system will increase damage done, decrease lock acquisition rate, etc.

### Shields
Ships will have shields that get depleted upon taking damage before hull damage starts to accumulate. I do like the Elite approach of lasers affecting shields more than ballistics, but hull less, so I think that's another concept that I'm going to steal wholesale. I like the concept of angling shields, but I think this is a system that will differ on a per-ship basis. Smaller ships might not have any zoned shielding, medium sized ships have a basic fore/rear shield setup, and larger ships might even have 4 different angles to manage - something that is probably too much for a single player to handle, hence the necessity for a copilot. An overcharged shield system will provide more resistance, and recharge faster.

## References / Influences
- Rogue Squadron: https://en.wikipedia.org/wiki/Star_Wars:_Rogue_Squadron
- Star Wars: Starfighter: https://en.wikipedia.org/wiki/Star_Wars:_Starfighter
- Star Wars Battlefront 2 (starfighter sections): https://en.wikipedia.org/wiki/Star_Wars_Battlefront_II_(2017_video_game)
- Lovers in a Dangerous Spacetime: https://en.wikipedia.org/wiki/Lovers_in_a_Dangerous_Spacetime
- Bomber Crew: https://en.wikipedia.org/wiki/Bomber_Crew
- Defender (Original XBox version): https://xboxaddict.com/game-profile/Xbox/260/Defender.html
- Star Wars Squadrons: https://en.wikipedia.org/wiki/Star_Wars:_Squadrons
- Elite Dangerous: https://en.wikipedia.org/wiki/Elite_Dangerous