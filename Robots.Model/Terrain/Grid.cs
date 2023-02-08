using Robots.Model.Interfaces;

namespace Robots.Model.Terrain;

public sealed class Grid : IGrid
{
    public ICell[][] Cells { get; }
    public Location Bounds { get; }

    public Grid(uint xSize, uint ySize)
    {
        Cells = new ICell[xSize][];
        for (uint x = 0; x < xSize; x++)
        {
            Cells[x] = new ICell[ySize];
            for (uint y = 0; y < ySize; y++)
            {
                var location = new Location(x, y);
                var cell = new Cell(this, location);
                Cells[x][y] = cell;
            }
        }

        Bounds = new Location(xSize, ySize);
    }
}