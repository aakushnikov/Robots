namespace Robots.Model.Terrain;

public struct Location
{
    public uint X { get; }
    public uint Y { get; }

    public Location(uint x, uint y)
    {
        X = x;
        Y = y;
    }
}