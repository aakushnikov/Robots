using Robots.Model.Interfaces;
using Robots.Model.Robot;

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

    public Location? CanMove(Location currentLocation, Command command, Direction direction)
    {
        switch (command)
        {
            case Command.Left:
            case Command.Right:
                throw new ArgumentException(
                    $"Can't use command {command} because it's not a movement command. ",
                    nameof(command));
            case Command.Forward:
                var cell = Cells[currentLocation.X][currentLocation.Y];
                var nextCell = cell.NextCells[(int)direction];
                return nextCell?.Location;
            default:
                throw new NotImplementedException($"Features for command {command} was not implemented");
        }
    }
}
