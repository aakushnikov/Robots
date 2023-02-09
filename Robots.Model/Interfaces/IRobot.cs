using Robots.Model.Robot;
using Robots.Model.Terrain;

namespace Robots.Model.Interfaces;

public interface IRobot
{
    void Process();
    void SetCommands(Command[] commands);
    Command[] RoadMap { get; }
    Location CurrentPosition { get; }
    Direction CurrentDirection { get; }
    Guid Id { get; }
    bool IsLost { get; }
}