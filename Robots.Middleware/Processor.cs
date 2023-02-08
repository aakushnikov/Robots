using Logging.Net;
using Robots.Model.Terrain;
using Robots.Model.Robot;
using Robots.Model.Interfaces;

namespace Robots.Middleware;

public partial class Processor
{
    public bool OkAccepted { get; private set; }
    
    private IGrid? _grid;
    private States _state;
    private readonly IList<IRobot> _robots;

    public Processor()
    {
        _robots = new List<IRobot>();
    }

    public bool ParseInstruction(string? input, out string error)
    {
        error = string.Empty;

        try
        {
            if (OkAccepted)
                throw new NotSupportedException("All commands were accepted. No more instructions are allowed");
            
            if (string.IsNullOrEmpty(input))
                throw new FormatException("Instruction cannot be empty");
            
            switch (_state)
            {
                case States.WaitingForGridData:
                    ParseGridData(input, out _grid);
                    _state = States.WaitingForRobotData1;
                    break;
                case States.WaitingForRobotData1:
                    if (CheckForOk(input))
                    {
                        _state = States.Ready;
                        return ParseInstruction(input, out error);
                    }

                    if (_grid == null)
                        throw new ApplicationException("Grid is null when it shouldn't be");

                    ParseLocationAndDirectionData(input, _grid, out var location, out var direction);
                    _state = States.WaitingForRobotData2;
                    break;
                case States.WaitingForRobotData2:
                    ParseCommandData(input, out var commands);
                    _state = States.WaitingForRobotData1;
                    break;
                case States.Ready:
                    OkAccepted = true;
                    break;
                default:
                    throw new NotImplementedException("Unknown state. Features was not implemented");
            }
        }
        catch (ApplicationException ex)
        {
            Logger.Error(ex);
        }
        catch (NotImplementedException ex)
        {
            Logger.Error(ex);
        }
        catch (Exception ex)
        {
            error = ex.Message;
            return false;
        }

        return true;
    }

    private static bool CheckForOk(string input)
    {
        return input.Equals("OK", StringComparison.OrdinalIgnoreCase);
    }
    
    private static void ParseGridData(string input, out IGrid grid)
    {
        var delimiter = Tools.Configuration.GetDelimiter();
        var data = input
            .Trim()
            .Split(delimiter, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .ToArray();

        if (data.Length != 2)
            throw new FormatException($"There should be to integers split by '{delimiter}'");

        grid = new Grid(uint.Parse(data[0]), uint.Parse(data[1]));
    }

    private static void ParseLocationAndDirectionData(string input, IGrid grid, out Location location, out Direction direction)
    {
        if (grid == null)
            throw new ApplicationException("Grid should be initialized first");
        
        var delimiter = Tools.Configuration.GetDelimiter();
        var data = input
            .Trim()
            .Split(delimiter, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .ToArray();

        if (data.Length != 3)
            throw new FormatException($"There should be two integers and one char split by '{delimiter}'");

        var x = uint.Parse(data[0]);
        var y = uint.Parse(data[1]);

        if (x >= grid.Bounds.X)
            throw new FormatException(
                $"X={x} cannot be larger then specified grid size {grid.Bounds.X}. Index assumed to start from 0");
        
        if (y >= grid.Bounds.Y)
            throw new FormatException(
                $"Y={y} cannot be larger then specified grid size {grid.Bounds.Y}. Index assumed to start from 0");
        
        location = new Location();
        direction = Enum.Parse<Direction>(
            Enum.GetNames<Direction>()
                .First(d => d.StartsWith(data[2], StringComparison.OrdinalIgnoreCase)));
    }

    private static void ParseCommandData(string input, out Command[] commands)
    {
        commands = input
            .Select(ch => Enum.Parse<Command>(
                Enum.GetNames<Command>()
                    .First(c => c.StartsWith(ch.ToString(), StringComparison.OrdinalIgnoreCase))))
            .ToArray();
    }
}