# MLAgentsSandbox

## Description
This project is a Sandbox I made to understand and learn how to use Machine Learning with the MLAgent library

Currently there are only a few project:

### BasicAgent
This agent learned how to go to a Goal (the green ball) without touching the walls (the red walls).

The process of learning was made 3 times:
1. With a static position of the Cube and a static position of the Goal
2. With a static position of the Cube and a random position of the Goal
3. With a random position of the Cube and a random position of the Goal

With the last learning environment, the cube learned to succesfully go to the Goal. Even if during runtime we move the Goal, the Cube will adjust is movement to follow and reach the new position of the Goal 


### Escape Rooms
This agent must learn how to complete some simple escapes room, the agent needs to see where the buttons are, press it and go outside the door as fast as he can.

#### Escape Room 1
This escape room is very simple, the Agent must learn to navigate the room and press the pressure plate to open the door, then go through the door and complete the level.

The result are handled this way:
- -0.01 each frame
- -0.5 each time he jumps (so he doesn't jump randomly)
- -0.1 each frame if he's stuck
- -2 if he's stuck for more than one second
- -0.4 if he hits a wall
- +2 each time a pressure plate is pressed
- -2 each time he falls
- +10 at level completed

More to come
