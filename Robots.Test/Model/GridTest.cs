using Robots.Model.Robot;
using Robots.Model.Terrain;

namespace Robots.Test.Model;

[TestFixture]
public class GridTest
{
    [Test]
    [TestCase(1, 1, 0, 0, Command.Left, Direction.North)]
    [TestCase(1, 1, 0, 0, Command.Right, Direction.North)]
    public void CannotChangeLocation_Exceptions(int xSize, int ySize, int xLocation, int yLocation, Command command, Direction direction)
    {
        var grid = new Grid((uint)xSize, (uint)ySize);
        Assert.Throws<ArgumentException>(() =>
            grid.CanChangeLocation(new Location((uint)xLocation, (uint)yLocation), command, direction));
    }

    public static object?[][] GridTestData = 
    {
        new object?[] { new Location(1, 1), new Location(0, 0), Command.Forward, Direction.North, null },
        new object?[] { new Location(2, 2), new Location(1, 1), Command.Forward, Direction.South, new Location(1, 0) },
    };

    [Test]
    [TestCaseSource(nameof(GridTestData))]
    public void CanChangeLocation_Well(Location bounds, Location currentLocation, Command command, Direction direction, Location? expectedLocation)
    {

        var grid = new Grid(bounds.X, bounds.Y);
        var result = grid.CanChangeLocation(currentLocation, command, direction);

        if (expectedLocation == null)
            Assert.IsNull(result);
        else
            Assert.AreEqual(expectedLocation, result);
    }
}