using Robots.Model.Robot;
using Robots.Model.Terrain;

namespace Robots.Model.Interfaces;

public interface IRobot
{
    void Process();
    Command[] RoadMap { get; }
    Location CurrentPosition { get; }
    Direction CurrentDirection { get; }
}