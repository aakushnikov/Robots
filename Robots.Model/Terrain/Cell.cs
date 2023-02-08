using Robots.Model.Interfaces;

namespace Robots.Model.Terrain;

public sealed class Cell : ICell
{
    public Location Location { get; }
    public IGrid Grid { get; }

    public Cell(IGrid grid, Location location)
    {
        Grid = grid;
        Location = location;
    }
}