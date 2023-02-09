using Robots.Model.Robot;
using Robots.Model.Terrain;

namespace Robots.Model.Interfaces;

public interface IGrid
{
    ICell[][] Cells { get; }
    Location Bounds { get; }
    Location? CanMove(Location currentLocation, Command command, Direction direction);
}