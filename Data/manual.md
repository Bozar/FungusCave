# Fungus Cave Manual

## Table of Contents

* Summary
* Potion
* Energy & UI
* Stress & Infection
* Fungus & Fog
* Power
* Key Binding

## Summary

Fungus Cave is a coffee break Roguelike game. A successful run takes about ten
minutes. Your goal is to fight through five dungeon levels and kill all enemies
that carries potion.

The game is made with C# and Unity. The ☼masterful☼ tileset, `curses_vector`, is
created by DragonDePlatino for [Dwarf
Fortress](http://www.bay12forums.com/smf/index.php?topic=161328.0). The color
theme, `One Dark Pro`, is created by binaryify for [Visual Studio
Code](https://marketplace.visualstudio.com/items?itemName=zhuangtongfa.Material-theme).

## Potion

Most enemies drop one or more bottles of potion when killed. The potion can be
used to buy powers. It will also be consumed automatically when your HP is less
than 1. On the other hand, you lose the game when you are killed with no potion
left.

## Energy & UI

Move and attack costs energy, which is a hidden element from player. You can
tell the flow of game time by examining turn indicator: `[ X X X | X ]`. Every
X represents one turn. An actor who has enough energy can take multiple actions
in one turn. The same actor might also be forced to wait one or more turns if he
is low on energy.

Diagonal actions cost more energy. Move out of pool costs more energy.
[WIP]Attack in fog costs more energy.[/WIP] The environment indicator shows your
surroundings: `[ @ | = | ? ]`. These symbols refer to enemy, pool and fog
respectively.

## Stress & Infection

An actor can only bump attack his adjacent target. Every attack has a chance to
infect the target. The target is more likely to be infected when he is standing
in a pool or has low HP.

PC's first three infections are counted as stress. The fourth infection actually
takes effect. Infecting an infected target increases the current infection's
duration, to a maxmium of 9 turns.

There are three types of infection:

* Slow: Move and attack costs more energy.
* Weak: Reduce actor's damage by 1.
* Mutated: NPC drops 1 more potion. PC revives with 5 HP (instead of 10).

## Fungus & Fog

Work in progress.

## Power

Powers take effect automatically following two steps. First, you need to
purchase a power with potions. Second, you have to accumulate stress to active
the power.

Defense, Energy:

* Vigor: Restore energy every turn.
* Adrenaline: Restore energy every turn. The restoration increases at low HP.
  Requires Vigor.

Defense, Infection:

* Immunity: Increase your max Stress by 1.
* Fast Heal: Infection duration is reduced by 2 (instead of 1) every turn.
  Requires Immunity.

Defense, HP:

* First Aid: Restore 1 HP every turn when your current HP is less than 5.
* Reaper: Restore 1 HP when you kill an enemy. Requires First Aid.

Attack:

* Siphon: Your target loses energy, which increases with your damage, when being
  hit.
* Plague: Your attack has a greater chance to cause infection.
* Bleed: Increase your damage by 1.

## Key Binding

Normal mode:

* [ hjklyubn | Num pad | Arrow keys ] -- Move in eight directions.
* [ . | Num pad 5 ] -- Wait 1 turn.
* [ o ] -- Auto explore.
* [ Space ] -- Confirm.
* [ Esc ] -- Exit or cancel.

Examine mode:

* [ x ] -- Enter Examine mode.
* [ o | d | PgDn] -- Lock next NPC in view.
* [ i | s | PgUp] -- Lock previous NPC in view.

Menu:

* [ c ] -- Open menu to buy powers.
* [ v | m ] -- Open menu to view log.
* [ / | ? ] -- Open menu to view help.
* [ Tab | Shift + Tab ] -- Switch between menus.

Some key-bindings and features are only available in Wizard Mode. Open
`Data/setting.xml`, set `IsWizard` to `true`. When in Wizard Mode, there is
a question mark before the version number in the lower right corner.

Wizard mode:

* Space (Normal Mode): Reload the game.
* o: Auto explore.
* p: Print PC's energy cost.
* q: Gain 1 potion.
* 1: Switch fog of war.
* 2: Print PC's current energy.
* 3: Gain energy for PC.

vim: set tw=80:
