# MLAgentsSandbox

## Description
This project is a Sandbox I made to understand and learn how to use Machine Learning with the MLAgent library.

Currently there are only a few project:

### BasicAgent
This agent learned how to go to a Goal (the green ball) without touching the walls (the red walls).

The process of learning was made 3 times:
1. With a static position of the Cube and a static position of the Goal.
2. With a static position of the Cube and a random position of the Goal.
3. With a random position of the Cube and a random position of the Goal.

With the last learning environment, the cube learned to succesfully go to the Goal. Even if during runtime we move the Goal, the Cube will adjust is movement to follow and reach the new position of the Goal .


### Escape Rooms
This agent must learn how to complete some simple escapes room, the agent needs to see where the buttons are, press it and go outside the door as fast as he can.

#### Escape Room 1
This escape room is very simple, the Agent must learn to navigate the room and press the pressure plate to open the door, then go through the door and complete the level.

The rewards are handled this way:
- -0.001 each step
- -0.02 each time the agent jumps
- -0.2 if he remains stuck for more than one second
- -0.04 if he hits an obstacle (wall)
- -0.035 each step if he keeps colliding with the obstacle
- +0.3 each time he steps on the pressure plate
- -0.2 if he presses the pressure plate going backward
- -0.2 each time he falls
- +1 each time he finishes the room

#### Escape Room 2
This escape room is very similar to the Escape Room 1, the only difference is that the pressure plate and the agent are swapped in place.

With this change the agents needs to learn to turn around and go to the pressure plate, before going into the exit gate.

The rewards are the same of the Escape Room 1.

More to come
