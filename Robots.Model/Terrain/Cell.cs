using Robots.Model.Interfaces;
using Robots.Model.Robot;

namespace Robots.Model.Terrain;

public sealed class Cell : ICell
{
    public Location Location { get; }
    public IGrid Grid { get; }
    public ICell?[] NextCells { get; }

    public Cell(IGrid grid, Location location)
    {
        Grid = grid;
        Location = location;

        NextCells = AllocateNextCells();
    }
    
    private ICell?[] AllocateNextCells()
    {
        var enumList = Enum.GetValues<Direction>();
        var cells = new ICell?[enumList.Length];
        foreach (var direction in enumList)
        {
            var x = Location.X;
            var y = Location.Y;
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
                    throw new NotImplementedException($"Features for direction '{direction}' was not implemented");
            }

            ICell? nextCell = null;
            try
            {
                nextCell = Grid.Cells[x][y];

            }
            catch
            {
                // ignored
            }

            cells[(int)direction] = nextCell;
        }

        return cells.ToArray();
    }
}