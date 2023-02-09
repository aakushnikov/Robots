using Robots.Model.Interfaces;
using Robots.Model.Robot;

namespace Robots.Model.Terrain;

public sealed class Grid : IGrid
{
    public Location Bounds { get; }

    public Grid(uint xSize, uint ySize)
    {
        Bounds = new Location(xSize, ySize);
    }

    public Location? CanChangeLocation(Location currentLocation, Command command, Direction direction)
    {
        switch (command)
        {
            case Command.Left:
            case Command.Right:
                throw new ArgumentException(
                    $"Can't use command {command} because it's not changing location",
                    nameof(command));
            case Command.Forward:
                return GetNextLocation(direction, currentLocation);
            default:
                throw new NotImplementedException(
                    $"Features for command {command} was not implemented");
        }
    }
    
    private Location? GetNextLocation(Direction direction, Location location)
    {
        var x = (int)location.X;
        var y = (int)location.Y;
        
        switch (direction)
        {
            case Direction.North:
                y++;
                break;
            case Direction.South:
                y--;
                break;
            case Direction.East:
                x++;
                break;
            case Direction.West:
                x--;
                break;
            default:
                throw new NotImplementedException(
                    $"Features for direction '{direction}' was not implemented");
        }
        
        if (y >= Bounds.Y || x >= Bounds.X || x < 0 || y < 0) return null;

        return new Location((uint)x, (uint)y);
    }
}
