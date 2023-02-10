Problem
The surface of Mars can be modelled by a rectangular grid around which robots are able to move according to instructions provided from Earth. You are to write a program that determines each sequence of robot positions and reports the final position of the robot.
A robot position consists of a grid coordinate (a pair of integers: x-coordinate followed by y-coordinate) and an orientation (N, S, E, W for north, south, east, and west).
A robot instruction is a string of the letters “L”, “R”, and “F” which represent, respectively, the instructions:
•Left: the robot turns left 90 degrees and remains on the current grid point.
•Right: the robot turns right 90 degrees and remains on the current grid point.
•Forward: the robot moves forward one grid point in the direction of the current orientation and maintains the same orientation. The direction North corresponds to the direction from grid point (x, y) to grid point (x, y+1). There is also a possibility that additional command types maybe required in the future and provision should be made for this.
Since the grid is rectangular and bounded, a robot that moves “off” an edge of the grid is lost forever. However, lost robots leave a robot “scent” that prohibits future robots from dropping off the world at the same grid point. The scent is left at the last grid position the robot occupied before disappearing over the edge. An instruction to move “off” the world from a grid point from which a robot has been previously lost is simply ignored by the current robot.

Input
The first line of input is the upper-right coordinates of the rectangular world, the lower-left coordinates are assumed to be 0,0.
The remaining input consists of a sequence of robot positions and instructions (two lines per robot).
A position consists of two integers specifying the initial coordinates of the robot and an orientation (N, S, E, W), all separated by white space on one line. A robot instruction is a string of the letters “L”, “R”, and “F” on one line.
Each robot is processed sequentially, i.e., finishes executing the robot instructions before the next robot begins execution.
The maximum value for any coordinate is 50. All instruction strings will be less than 100 characters in length.

Output
For each robot position/instruction in the input, the output should indicate the final grid position and orientation of the robot. If a robot falls off the edge of the grid the word “LOST” should be printed after the position and orientation.

Sample Input
5 3
1 1 E
RFRFRFRF
3 2 N
FRRFLLFFRRFLL
03 W
LLFFFLFLFL

Sample Output
11 E
3 3 N LOST
2 3 S

Solution overview
This solution design based on DDD-pattern. It has a model layer, middleware and simple UI (console) layer where each previous of them doesn't know anything about the next layer. And of course solution has a unit tests powered by NUnit. 
Model layer defines and describes a properties and some features of used entities. It has a Grid Interface (which is IGrid) and a Robot Interface (which is IRobot) to provide basic model and some features for this model. Also we have a bunch of classes that implements those interfaces.
Middleware provides:
- services functionality which is allow to work with application configuration (read settings)
- software functionality which is allow to provide some features to UI most simpler
UI layer provides a executable file which allow to run the application, receive some input instructions from a user and deliver some output data to a user:
- to set used grid size,
- to set robot's starting position,
- to set robot's commands,
- to run robot's processing,
- to get robot's final position on related grid.

Assumptions that was used:
- application can't crash
- application can't hang
- grid can be decomposed to cells, but it's not necessary on this step
- more IO provides should be designed for any new UI
- commands and directions can be implemented as a classes, but it's not necessary on this step
- processor should support object instructions, but it's not necessary on this step
- logging can be added later
- each test method was designed to cover related feature as most as possible, but not all application functionality was covered by tests.

All entire solution need a little bit more 'sugar' code before delivering to a customer.
If suppose that there no more requirement and details for this task delivering and only software should be delivered (without any installation, resource sharing and user studying) so it can take a couple of days more. 