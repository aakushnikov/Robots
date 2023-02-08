using Robots.Model.Terrain;

namespace Robots.Model.Interfaces;

public interface ICell
{
    Location Location { get; }
    IGrid Grid { get; }
}