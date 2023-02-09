using Robots.Model.Interfaces;
using Robots.Model.Terrain;

namespace Robots.Model.Robot;

public class Robot : IRobot
{
    public Command[] RoadMap { get; private set; }
    public Location CurrentPosition { get; private set; }
    public Direction CurrentDirection { get; private set; }
    public Guid Id { get; }
    
    public bool IsLost { get; }

    public Robot(Location location, Direction direction)
    {
        CurrentPosition = location;
        CurrentDirection = direction;
        RoadMap = Array.Empty<Command>();
        Id = Guid.NewGuid();
    }
    
    public void Process()
    {
        // TODO
    }

    public void SetCommands(Command[] commands)
    {
        RoadMap = commands;
    }
}