namespace Robots.Middleware;

public partial class Processor
{
    public enum States
    {
        WaitingForGridData,
        WaitingForRobotData1,
        WaitingForRobotData2,
        Ready,
    }
}