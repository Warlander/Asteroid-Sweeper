# Asteroid Sweeper

This is a small game heavily inspired by Minesweeper, created in approximately 13 real-time dev hours from scratch to the first release candidate version pushed to this repository - including game design, searching for the right art assets and all the code.

## Features

* All traditional features you can expect in a game inspired by Minesweeper (revealing tiles, marking tiles, mines - called asteroids here, showing surrounding asteroid count)
* Random generation of boards with varying amount of mines and sizes.
* Quick restart on the same board option.
* Remaining asteroids tracking.
* Support for custom JSON boards.
* Automatic tile reveal for tiles with no asteroids around.
* Auto-move and auto-play.
* "Saving grace" feature if you hit mine in a first move, which automatically reveals 3X3 square.
* Polished and well-animated UI and gameplay.
* Support for any rectangle-shaped boards.

## Project notes

* If something isn't explained here, it's very likely explained with code comments in the project, property/variable naming or tooltips on properties - everything about the project I thought requires some additional explainations is explained in one or more of these ways.
* Editor version: 2021.3.14f1
* Project extensively uses **Dependency Injection** framework **Extenject**, some interesting Extenject features and power it can give are also showcased in the project.
* Model and view are strictly separated - model can work headless or use different views and controllers, which makes automated testing (which was out of scope of this project) trivial, especially given there's already AutoPlayer which after some small additions could be used for this purpose.
* Project uses **DOTween** for all animations. Many of them are customizable via properties on MonoBehaviours.
* MonoBehaviours have properties for prefabs, UI elements and other important values exposed. For more utility-centric ones, if no value is provided, default most obvious value will be used instead. (typically from the same GameObject)
* I expose buttons as properties and never Unity callbacks - this is deliberate code and project design choice, both for cleaner and more traceable code as well as uniformity of editor use.
* **Saving Grace** is a feature I came up with to turn frustration of hitting a mine in a first move on a pre-defined board into beneficial, satisfying event. Saving grace can be turned off in GameConfig (in Prefabs).
* I left art assets and plugins as intact as possible in their original folders, in longer term project it makes it easier to upgrade if new version of external assets comes out. Art assets are organised in folders corresponding to their primary use.
* I decided to put all art into a single sprite atlas instead of using separate sprite atlases - all sprites in the project take only a single 1024X1024 atlas, so there is no need to divide it further like in a larger project.
* I was considering both UI-only and UI/2D game scene setup, the project of this scope could be done faster with UI-only approach but I decided to use mixed approach instead so there's showcase of both being used at the same time, as well as use of more Unity features overall. However, main menu animation is fully UI-based.
* Auto-move and auto-play never cheats, they will only use info available to the player and will NEVER make any move which is a guess - this includes first and often last few moves. If no move can be performed, both buttons will respond with visual feedback.
* Restart option restarts the current board, for both predefined and randomly generated maps.
* Random levels are customizable via main menu random level buttons, RandomLevelButton.
