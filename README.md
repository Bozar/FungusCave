# Fungus Cave

## Summary

Fungus Cave is a Roguelike game made with Unity. Explore the cave, fight enemies, and find Encyclopedia of Yendor to cure your infections. The ☼masterful☼ tileset, `curses_vector`, is created by DragonDePlatino for [Dwarf Fortress](http://www.bay12forums.com/smf/index.php?topic=161328.0). The color theme, `One Dark Pro`, is created by binaryify for [Visual Studio Code](https://marketplace.visualstudio.com/items?itemName=zhuangtongfa.Material-theme).

> '... Blood,' he continued after a pause, 'blood is holy! Blood does not see the light of God's sun, blood is hidden from the light ... And a great sin it is to show blood to the light of day, a great sin and cause to be fearful, oh, a great one it is!'
>
> -- Turgenev, Ivan. Sketches from a Hunter's Album.
>
> Extract blood from a sleeping patient when his body is still warm and soft. Such extracted essence can be used to keep you awake in times of emergency.
>
> -- Pages from Encyclopedia of Yendor.

Fungus Cave is still under development. There is only one dungeon level at the moment. Kill all the enemies who have loot to win the game.

## Key-bindings

Normal mode:

* h/j/k/l/y/u/b/n, Number pad, Arrow keys: Move around.
* ., Number pad 5: Wait 1 turn.
* x: Enter examine mode.

Examine mode:

* h/j/k/l/y/u/b/n, Number pad, Arrow keys: Move around.
* o/d/PgDn: Lock next target.
* i/s/PgUp: Lock previous target.
* Esc: Exit examine mode.

Some key-bindings and features are only available in Wizard Mode. Open `Data/test.xml`, set `IsWizard` to `true`. When in Wizard Mode, there is a question mark before the version number in the lower right corner.

Wizard mode:

* c: Open the menu to buy powers.
* Space (Buy power): Buy a new power.
* Space (Normal Mode): Reload the game.
* o: Auto explore.
* p: Print PC's energy cost.
* q: Gain 1 potion.
* 1: Switch fog of war.
* 2: Print PC's current energy.
* 3: Gain energy for PC.

## Infections

Attacks are guarenteed to hit and they have a chance to infect the target. The first three infections are counted as Stress, which is used to unlock Powers. The fourth infection actually takes effect. Any more infections will increase the duration of your current infection.

There are three types of infections:

* Slow: Move and attack costs more energy.
* Mutated: You can only restore 5 HP (instead of 10) after reviving. Muatated enemies drop 1 more potion.
* Weak: Decrease damage by 1.

