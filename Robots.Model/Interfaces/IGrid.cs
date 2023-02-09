using Robots.Model.Robot;
using Robots.Model.Terrain;

namespace Robots.Model.Interfaces;

public interface IGrid
{
    Location Bounds { get; }
    Location? CanChangeLocation(Location currentLocation, Command command, Direction direction);
}