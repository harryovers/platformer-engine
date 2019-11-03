# Platformer Game Engine
## What?
A 2D game engine that was created solely for making platformer games, although it can be used for other styles of games. The code is written in C#.

There are two unnamed games included in this repository.
  - #### Platform.Game
  A multi-room platforming game that contains one level. The levels layouts are stored in bitmaps where each colour represents a diffent game element.
  - #### Twin.Game
  A twin-stick shooter, this game is the early stages of an experiment to see if the game engine can be adapted to other styles of 2D games.

## Why?
I had a couple of ideas for games that I wanted to explore and had some thoughts about what I would like in a game engine to facilitate this process. There are game engines that would have allowed me to do this but I fancied writing one for myself.

## When?
The project started in the second half of 2015 and work continued until around September 2016.

## Details

### Packages
- Physics.Sandbox - Used in early development of the physics engine.
- Platform.Console - Used to test how elements of the game react with eachother and give feedback for debugging.
- Platform.Core - This contains some shared classes used throughout the game engine.
- Platform.Entities - Has all of the elements that are used within the platform game. Combines physics and rendering of sprites along with data about each element.
- Platform.Game - The main entry-point for the platformer game.
- Platform.Gameplay - Gameplay logic and state management.
- Platform.Levels - Contains just one class that takes a bitmap, parses it and hold all the data that is needed to use the level.
- Platform.Maths - This was never used but was intended to be a package to handle all number calculations that were needed for the game engine.
- Platform.Physics - A very simple rigid body physics engine based on some of the concepts found in Box2D but simplified for use within the limited scope of this game engine.
- Twin.Game - An experiment to see if the game engine could be used to make a twin-stick shooter.

### Stuff Missing
There are two major parts missing from the code. There are no test as most of the testing was done with gameplay, given how the code is structured it would not be difficult to add unit tests. The other majory thing missing is the lack of comments in the majority of the code.

### Future Work
There are currently no plans to do any futher work on this.

