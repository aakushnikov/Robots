using Robots.Model.Terrain;

namespace Robots.Model.Interfaces;

public interface IGrid
{
    ICell[][] Cells { get; }
    Location Bounds { get; }
}