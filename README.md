# SpaceShooter

The goal for this project is to make a cooperative-multiplayer focused space shooter game in a vein similar to the old Rogue Squadron or Star Fox games.

## Setting
The players are pilots that are part of a Ragtag Defense Force (RDF) that is protecting the galaxy from the Legion of Evil. The RDF is always low on resources, so the pilots have to jump from planet to planet, helping where they can, but with little assistance from above.

## Style
Despite the nature of the RDF, the style of all their ships is Retro-Raygun inspired. Individual fighter ships look like 1950s concept cars as space fighters. The players' ships in particular are hot-rodded out, which provides a juxtaposition to the sleek smooth designs of civilian (or less experienced military) craft.

The Legion of Evil has similarly retro-raygun styled craft as well, but more angular and with more spikes and ridges. Their craft are more airplane-inspired than those of the RDF.

## Wants list
The game should be
- Cooperative multiplayer up to 6 players
- Co-op focused, with specific features that make the game more fun to play as a team
- Level-based
- Extendable! Ships and levels should be addable easily by players

## Unique Selling Point / Elevator Pitch
Take well loved space shooter gameplay, but give it a new dimension via a co-op experience that has players working together.
"Imagine Star Fox, if you and your friends were all in the Millennium Falcon together!"

## Levels
Levels should be self-contained units, with different objectives that the players need to achieve in order to succeed. They should follow a "free world" format. For the time being, despite being a "space" shooter, I forsee most of the levels taking place on the surface of planets, moons, etc, to give the players a sense of orientation via the ground.

### Objectives
This is a short list of some of the objectives I would expect players might have to fulfill in order to progress through the level.
- Survive: A bunch of enemies spawn, players have to defeat all of them (or at least some percentage of them) before moving on
- Destroy: There's an enemy building/ship/whatever, and it needs to get blown up. Probably on a timer or something
- Protect: There's a friendly building/ship/whatever, and it needs to not get blown up.
- Get to point X: Useful for transitioning between set pieces
- Pick up/Deliver: Certain ground-based NPCs can be picked up and carried. If the player gets them close to the ground, they can be dropped off.

### Level structure
The level drops players in at the same relative location, and there is a chain of objectives and events that they need to complete in order to complete their mission. All players share the same state in the level progression chain, there are no secret player agendas or anything like that. Though maybe some players might be searching for an achievement or something like that. It might be fun to have secret hidden objectives, but with the open-ended concept I don't think that would be as interesting.

I don't think there will be a concept of "bosses" like Star Fox, but there might be some souped-up enemies that take on that role.

## Ships
Ships should have a few manageable stats, and some tweaks that can be applied to those stats. They can also have different loadouts which can be preselected before starting a level.

Some ships should be multi-player. One pilot flies the ship, and other players can be gunners, bomb experts(?), or copilots.

### Ship Types
- Workhorse, jack-of-all trades fighter. 1 seat
- Speedy interceptor. 1 seat
- Missile Boat. 1 Seat
- Gunship with turret and bombs. 2 seats - pilot and gunner
- Landing craft. 3 Seats - Pilot, copilot, tail gunner
- War bomber. 4 Seats - Pilot, copilot, nose gunner, tail gunner

### Ship Stats

Stats include:
- Hit points
- Maximum speed at each power level (AEPL)
- Rate of rotation in each axis AEPL
- Rate of boost charge AEPL
- Boost capacity
- Boost speed when being used
- Boost usage per second while being used
- Weapon system recharge AEPL
- Weapon bonus modifier for overcharged system AEPL
- Shield capacity - not based on power level!
- Shield recharge rate AEPL
- Shield recharge time AEPL. The amount of time after getting hit before the shield starts regenerating
- Number and layout of Shield zones
- Weapon slots

## Flight
I'm really torn on whether to use a more flight-sim style flight setup where the player can control roll/pitch/yaw on their own, or if the more cinematic style used by most of my references is the way to go. The former will be easier to develop as it doesn't require complicated camera controls. However, for the style of gameplay I'm going for I think it might be too mentally taxing for the player and won't be as fun.

### Flight sim
- Requires roll, pitch, and yaw controls, hard on a gamepad
- No clamps
- Easy to code

### Cinematic Follow
- Oriented to the ground
- Handled by just one joystick
- vertical movement is clamped so the ship can't flip around
Almost like the joystick is moving the reticle, and the ship follows.

### Collisions
Running into things with the player ship will damage them. Depending on the type of collision, different effects may happen:
- Clipped collision: Secondary part of the ship hits something. Small amount of damage, temporarily disable that collider for a brief period. No re-orientation
- Glancing collision: Primary part of the ship hits something, but the path forward is still relatively clear (determined by what??). Moderate damage, possible re-orientation.
- Head-on collision: Primary part of the ship hits something, but there is no way forward. The ship is totally destroyed.
Note: Speed should play a part in this. Even a head-on collision at 1 m/s shouldn't be a complete game over... Will need to think about this some more. 

### Flight Systems
I do want the player to have to manage some basic flight systems. I like the Weapons/Engine/Shields setup used by Squadrons and Elite Dangerous, so I think I'm going to steal that approach wholesale. Players will have a set amound of energy "pips" that the ship has access to, and they can dynamically shift between a balanced power setup to favoring one system over the others in order to quickly charge it or gain a bonus.

### Engines
Ships will have a top speed that they can reach at each level of engine power. They will also have manuverability limits that change based on their current velocity. I _think_ that it will be fine to come to a full stop, instead of enforcing forward motion at all times, but that's not a decision I've settled on yet.

Like in Squadrons, I think it will be a nice gameplay element to have a boost, though unlike Squadrons I think the boost will constantly accumulate over time, but charge faster/last longer at higher power levels. I do _not_ intend to add any form of ship drifting like Squadrons, though.

### Weapons
All weapons will consume weapon power when fired, and if the weapons systems don't have power then they won't fire/acquire target locks, etc. Having an overcharged weapons system will increase damage done, decrease lock acquisition rate, etc. I do like the Elite approach of energy weapons affecting shields more than ballistics, but hull less, so I think that's another concept that I'm going to steal wholesale.

#### Weapon types
- Minigun: Ballistic, high RoF low damage per shot
- Flak: Ballistic, medium RoF, damage falloff over distance to represent spread
- Cannon: Ballistic, low Rof, high damage per shot
- Laser: Energy beam, continuous damage when hit
- Plasma: Energy bolt, slower but higher damage
- Static: Energy cone, starts doing low damage, but increases with time-on-target
- Tracking missiles: Seeking and fast, but have to lock-on first, and can be avoided
- Dumbfire rockets: Swarm of medium-damage projectiles
- Torpedoes: Lock-on, but too slow to be useful against fighter craft. For objectives only
- Bombs: Dropped, unguided, for objectives only. Or an achievement for dropping one on an enemy 

### Shields
Ships will have shields that get depleted upon taking damage before hull damage starts to accumulate.  I like the concept of angling shields, but I think this is a system that will differ on a per-ship basis. Smaller ships might not have any zoned shielding, medium sized ships have a basic fore/rear shield setup, and larger ships might even have 4 different angles to manage - something that is probably too much for a single player to handle, hence the necessity for a copilot. An overcharged shield system will provide more resistance, and recharge faster.

## Multiplayer Features
Larger Ships:
- Copilot: On larger ships, you are responsible for managing the power needs of the ship. You also direct the shields, to keep them angled for maximum effect. You can view a map of the battlefield, and set specific waypoints to make things easier for the pilot. You can also have a full 360 view of the ship to help the pilot.
- Gunner: On ships which have gun turrets, one player can sit in the seat and take direct control over them. This frees the pilot up to focus on flying, and allows for better strafing runs and overall combat effectiveness.

Wingmates: When smaller ships are joined in a wing, they get some benefits:
- When firing at an enemy that has a another wingmate in their sights, you get a damage boost.
- When flying in formation with your wingmates, you get boosted up to the highest speed of the ships in formation

## Tech Approach
Since this a cooperative game instead of competitive, using dedicated servers probably isn't necessarily required. Client relay should be acceptable because the chance of a bad actor affecting gameplay is much lower. Whether using servers or relay, though, low latency is going to be necessary, since the players are going to be constantly in motion and can fly close to each other. All sorts of tricks will be needed to smooth over even normal operating latency. Keeping the number of networked entities to a minimum will help, so as to keep the traffic volume lower.

Since the game state is likely to have more changing frame-by-frame than staying the same, it makes sense for each (flying) entity being synced over the wire to send its state out each frame, over an unreliable channel. Including a synced timestamp value allows the receivers to disregard packets if they have a newer state. Things to be sent include:
- Position (Vec3)
- Orientation (Vec3?)
- Speed (float, assuming that ships can only move forward or backwards would save from having to send a full vec3)

Other things like hp and damage state might be able to sent separately over a reliable channel, since that likely won't be changing every single tick.

### Weapons
Some weapons need very little information: just the state of the trigger, and their own internal mechanisms, like a full-auto gatling gun. Others need more information about the source ship. Its orientation, position in the world, etc. The ship doesn't need to know anything about the weapons, though. It just needs to provide them with power. 

## References / Influences
- Rogue Squadron: https://en.wikipedia.org/wiki/Star_Wars:_Rogue_Squadron
- Star Wars: Starfighter: https://en.wikipedia.org/wiki/Star_Wars:_Starfighter
- Star Wars Battlefront 2 (starfighter sections): https://en.wikipedia.org/wiki/Star_Wars_Battlefront_II_(2017_video_game)
- Lovers in a Dangerous Spacetime: https://en.wikipedia.org/wiki/Lovers_in_a_Dangerous_Spacetime
- Bomber Crew: https://en.wikipedia.org/wiki/Bomber_Crew
- Defender (Original XBox version): https://xboxaddict.com/game-profile/Xbox/260/Defender.html
- Star Wars Squadrons: https://en.wikipedia.org/wiki/Star_Wars:_Squadrons
- Elite Dangerous: https://en.wikipedia.org/wiki/Elite_Dangerous