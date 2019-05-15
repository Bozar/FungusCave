# Fungus Cave

## Summary

Fungus Cave is a coffee break Roguelike game. A successful run takes about fifteen minutes. Your goal is to fight through three dungeon levels and kill all enemies that carries potion. The game is made with C# and Unity. The ☼masterful☼ tileset, `curses_vector`, is created by DragonDePlatino for [Dwarf Fortress](http://www.bay12forums.com/smf/index.php?topic=161328.0). The color theme, `One Dark Pro`, is created by binaryify for [Visual Studio Code](https://marketplace.visualstudio.com/items?itemName=zhuangtongfa.Material-theme).

This readme serves as a quick start guide to the game. Please refer to `manual.md`, which is in the same place as `FungusCave.exe`, for more information. You might be interested in [other games](https://github.com/Bozar/DevBlog/wiki/GameList) made by me.

> '... Blood,' he continued after a pause, 'blood is holy! Blood does not see the light of God's sun, blood is hidden from the light ... And a great sin it is to show blood to the light of day, a great sin and cause to be fearful, oh, a great one it is!'
>
> -- Turgenev, Ivan. Sketches from a Hunter's Album.
>
> Extract blood from a sleeping patient when his body is still warm and soft. Such extracted essence can be used to keep you awake in times of emergency.
>
> -- Pages from Encyclopedia of Yendor.

## Key Binding

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

## Infection

* Slow: Move and attack costs more energy.
* Weak: Reduce actor's damage by 1.
* Mutated: NPC drops 1 more potion. PC revives with 5 HP (instead of 10).

