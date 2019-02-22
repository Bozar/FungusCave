# FungusCave

## Summary

Fungus Cave is a Roguelike game made with Unity. Explore the cave, fight enemies, and find Encyclopedia of Yendor to cure your infections. The ☼masterful☼ tileset, `curses_vector`, is created by DragonDePlatino for [Dwarf Fortress](http://www.bay12forums.com/smf/index.php?topic=161328.0). The color theme, `One Dark Pro`, is created by binaryify for [Visual Studio Code](https://marketplace.visualstudio.com/items?itemName=zhuangtongfa.Material-theme).

> '... Blood,' he continued after a pause, 'blood is holy! Blood does not see the light of God's sun, blood is hidden from the light ... And a great sin it is to show blood to the light of day, a great sin and cause to be fearful, oh, a great one it is!'
>
> -- Turgenev, Ivan. Sketches from a Hunter's Album.
>
> Extract blood from a sleeping patient when his body is still warm and soft. Such extracted essence can be used to keep you awake in times of emergency.
>
> -- Pages from Encyclopedia of Yendor.

## About Version 0.0.2

Fungus Cave is still under development. Version 0.0.2 is more like a tech demo than a playable game. There are 10 dummies in the dungeon. Try to kill them all. You can press Space at any time to reload the game.

## Key-bindings

Normal mode:

* h/j/k/l/y/u/b/n, Number pad, Arrow keys: Move around.
* .: Wait 1 turn.
* o: Auto explore.
* x: Enter examine mode.
* Space: Reload the game.

Examine mode:

* h/j/k/l, Number pad, Arrow keys: Move around.
* n/o/PgDn: Lock next target.
* p/i/PgUp: Lock previous target.
* Esc: Exit examine mode.

Wizard mode:

* 1: Switch fog of war.
* 2: Print PC's current energy.
* 3: Gain energy for PC.
* p: Print PC's energy cost.

## Powers

You gain 3 random powers at the start of each game. This is for testing only. Powers have to be purchased with potions in the actual game.

Defense, energy:

* Vigor: Restore a fixed amount of energy every turn.
* Adrenaline: Restore energy every turn. The restoration increases at low HP.

Defense, HP:

* First Aid: Restore 1 HP every turn if your current HP is less than 4.
* Reaper: Restore 2 HP whenever you kill an enemy.

Defense, infection:

* Immunity: You are less likely to be infected.
* Fast Heal: Infections last 2 turns, instead of 5.

Attack powers:

* Siphon: Attack drains enemy's energy, which increases with your damage.
* Plague: Attack is more likely to infect enemy.
* Bleed: Increase damage by 1.

## Infections

Attacks are guarenteed to hit and they have a chance to infect the target. The first three infections are counted as Stress, which is used to unlock Powers. The fourth infection actually takes effect. Any more infections will drain your energy.

There are three types of infections:

* Slow: Move and attack costs more energy.
* Poisoned: You are more likely to be infected.
* Weak: Decrease damage by 1.

