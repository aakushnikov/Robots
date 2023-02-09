using Robots.Model.Interfaces;
using Robots.Model.Terrain;

namespace Robots.Model.Robot;

public class Robot : IRobot
{
    private bool _processed;
    private readonly IGrid _grid;
    
    public Command[] RoadMap { get; private set; }
    public Location CurrentPosition { get; private set; }
    public Direction CurrentDirection { get; private set; }
    public Guid Id { get; }
    public bool IsLost { get; private set; }

    public Robot(IGrid grid, Location location, Direction direction)
    {
        _grid = grid;
        CurrentPosition = location;
        CurrentDirection = direction;
        RoadMap = Array.Empty<Command>();
        Id = Guid.NewGuid();
    }
    
    public void Process()
    {
        if (_processed) return;
        
        if (RoadMap.Length == 0) return;

        foreach (var cmd in RoadMap)
        {
            if (!ExecuteCommand(cmd)) break;
        }
        
        _processed = true;
    }

    private bool ExecuteCommand(Command command)
    {
        switch (command)
        {
            case Command.Left:
            case Command.Right:
                CurrentDirection = this.GetNewDirection(command);
                return true;
            case Command.Forward:
                var newLocation =_grid.CanChangeLocation(CurrentPosition, command, CurrentDirection);
                if (newLocation == null)
                {
                    IsLost = true;
                    return false;
                }

                CurrentPosition = newLocation.Value;
                return true;
            default:
                throw new NotImplementedException($"Features for command {command} was not implemented");
        }
    }

    public void SetCommands(Command[] commands)
    {
        RoadMap = commands ?? throw new ArgumentNullException(nameof(commands));
    }
}