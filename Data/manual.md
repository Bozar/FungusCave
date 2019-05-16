# Fungus Cave Manual

## Table of Contents

* Summary
* Game Mode
* Potion
* Energy
* Stress & Infection
* Power
* Fungus & Beetles
* Environment Indicator & Turn Indicator
* Change Settings
* Key Bindings
* Survival Tips

## Summary

Fungus Cave is a coffee break Roguelike game. A successful run takes about fifteen minutes. Your goal is to fight through three dungeon levels and kill all enemies that carries potion. The game is made with C# and Unity. The ☼masterful☼ tileset, `curses_vector`, is created by DragonDePlatino for [Dwarf Fortress](http://www.bay12forums.com/smf/index.php?topic=161328.0). The color theme, `One Dark Pro`, is created by binaryify for [Visual Studio Code](https://marketplace.visualstudio.com/items?itemName=zhuangtongfa.Material-theme).

You might be interested in [other games](https://github.com/Bozar/DevBlog/wiki/GameList) made by me.

## Game Mode

There are two game modes: Normal Mode and Dungeon Rush Mode. You can enable Rush Mode in the setting menu. In Normal Mode, you have to beat three dungeon. In Rush Mode, there is only one level 3 dungeon. If you can easily pass the first two dungeons, play Rush Mode can save your time.

## Potion

Most enemies drop one or more bottles of potion when killed. The potion can be used to buy powers. It will also be consumed automatically when your HP is less than 1. On the other hand, you lose the game when you are killed with no potion left.

## Energy

Move and attack costs energy, which is a hidden element from player. An actor who has enough energy can take multiple actions in one turn. The same actor might also be forced to wait one or more turns if he is low on energy. Diagonal actions cost more energy. Move out of pool costs more energy.

## Stress & Infection

An actor can only bump attack his adjacent target. Every attack has a chance to infect the target. The target is more likely to be infected when he is standing in a pool or has low HP.

PC's first three infections are counted as stress. The fourth infection actually takes effect. Infecting an infected target increases the current infection's duration to a maxmium of 9 turns.

There are three types of infection:

* Slow: Move and attack costs more energy.
* Weak: Reduce actor's damage by 1.
* Mutated: NPC drops 1 more potion. PC revives with 5 HP (instead of 10).

## Power

Powers take effect automatically following two steps. First, you need to purchase a power with potions. Second, you have to accumulate stress to active the power.

Defense, Energy:

* Vigor: Restore energy every turn.
* Adrenaline: Restore energy every turn. The restoration increases at low HP. Requires Vigor.

Defense, Infection:

* Immunity: Increase your max Stress by 1.
* Fast Heal: Infection duration is reduced by 2 (instead of 1) every turn. Requires Immunity.

Defense, HP:

* First Aid: Restore 1 HP every turn if your current HP is less than 5.
* Patient Zero: Restore 1 HP every turn if you are infected and your current HP is less than 8. Requires First Aid.

Attack:

* Siphon: Your target loses energy, which increases with your damage, when being hit.
* Plague: Your attack has a greater chance to cause infection.
* Bleed: Increase your damage by 1.

## Fungus & Beetles

There is an internal counter in the game. When it counts down to zero, the game will spawn four beetles around PC. Whenever you kill an enemy, excluding beetles, the counter decreases by 1. If the victim is adjacent to fungus(♣), the counter decreases by 2. Reviving yourself with potion IS counted as a kill. You will get a forewarning when beetles are about to emerge from pool.

## Environment Indicator & Turn Indicator

The environment indicator, which is on the right side of the screen and just below `Damage`, shows your surroundings: `[ @ | = ]`. These symbols refer to enemy and pool respectively. You can tell the flow of game time by examining turn indicator: `[ X X X | X ]`. Every X represents one turn.

## Change Settings

You can edit `setting.xml` to suit your play style. The file is located in the same place as `FungusCave.exe`. There is also a backup file, `Data/setting.xml`. If you mess things up, copy `Data/setting.xml` to the root folder of the game. DO NOT modify `Data/setting.xml` directly.

## Key Bindings

Normal mode:

* [ hjklyubn | Num pad | Arrow keys ] -- Move in eight directions.
* [ . | Num pad 5 ] -- Wait 1 turn.
* [ o ] -- Auto explore.
* [ Space ] -- Confirm.
* [ Esc ] -- Exit or cancel.
* [ Ctrl + S ] -- Save and exit the game.

Examine mode:

* [ x ] -- Enter Examine mode.
* [ o | d | PgDn] -- Lock next NPC in view.
* [ i | s | PgUp] -- Lock previous NPC in view.

Menu:

* [ c ] -- Open menu to buy powers.
* [ v | m ] -- Open menu to view log.
* [ / | ? ] -- Open menu to view help.
* [ Tab | Shift + Tab ] -- Switch between menus.

Some key bindings and features are only available in Wizard Mode. Open `setting.xml`, set `IsWizard` to `true`. When in Wizard Mode, there is a question mark before the version number in the lower right corner.

Wizard mode:

* [ Space ] -- Reload the game.
* [ p ] -- Print PC's energy cost.
* [ q ] -- Gain 1 potion.
* [ 1 ] -- Switch fog of war.
* [ 2 ] -- Print PC's current energy.
* [ 3 ] -- Gain energy for PC.

## Survival Tips

* Save potions for later use. Do not use potions to buy a new power unless you are adjacent to an enemy and you can kill the target in one hit.
* Enemies can see 1 grid further than PC. Wait 1 turn before venturing into unexplored area.
* Move 1 step at a time. Do not rely too much on auto exploration.
* Always make sure there is a clear path for retreat. By clear, I mean you can move in a straight line and there is no pool in the way.
* Avoid the pool at all costs.
* Try not to move or attack diagonally.
* Try not to kill an enemy who is adjacent to fungus.

