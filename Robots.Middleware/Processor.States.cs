namespace Robots.Middleware;

public partial class Processor
{
    private enum States
    {
        WaitingForGridData,
        WaitingForRobotData1,
        WaitingForRobotData2,
        Ready,
    }
}