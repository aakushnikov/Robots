using Robots.Model.Interfaces;

namespace Robots.Model.Robot;

public static class CommandToDirections
{
    private static readonly IDictionary<Command, IDictionary<Direction, Direction>> CommandsDictionary;
    
    static CommandToDirections()
    {
        CommandsDictionary = new Dictionary<Command, IDictionary<Direction, Direction>>
        {
            {
                Command.Left, new Dictionary<Direction, Direction>
                {
                    { Direction.North, Direction.West },
                    { Direction.West, Direction.South },
                    { Direction.South, Direction.East },
                    { Direction.East, Direction.North },
                }
            },
            {
                Command.Right, new Dictionary<Direction, Direction>
                {
                    { Direction.North, Direction.East },
                    { Direction.East, Direction.South },
                    { Direction.South, Direction.West },
                    { Direction.West, Direction.North },
                }
            }
        };
    }

    public static Direction GetNewDirection(this IRobot robot, Command command)
    {
        return !CommandsDictionary.ContainsKey(command) ? robot.CurrentDirection : CommandsDictionary[command][robot.CurrentDirection];
    }
}