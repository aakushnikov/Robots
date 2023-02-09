using Logging.Net;
using Robots.Middleware.Interfaces;
using Robots.Model.Terrain;
using Robots.Model.Robot;
using Robots.Model.Interfaces;

namespace Robots.Middleware;

public partial class Processor : IDisposable
{
    private bool _okAccepted;
    private IGrid? _grid;
    private States _currentState;
    private readonly IList<IRobot> _robots;
    private readonly IIOProvider _ioProvider;

    public Processor(IIOProvider ioProvider)
    {
        _robots = new List<IRobot>();
        _ioProvider = ioProvider;
    }

    private void Run()
    {
        if (!_okAccepted)
            throw new ApplicationException($"Grid and Robots locations and directions should be defined first. Use {nameof(ProcessInput)} method");
        _ioProvider.WriteLine();
        _ioProvider.WriteLine("Executing robots processing...");
        foreach (var robot in _robots)
        {
            robot.Process();
        }        
    }

    #region Parsers

    private bool ParseInstruction(string? input, out string error)
    {
        error = string.Empty;

        try
        {
            if (_okAccepted)
                throw new NotSupportedException("All commands were accepted. No more instructions are allowed");
            
            if (string.IsNullOrEmpty(input))
                throw new FormatException("Instruction cannot be empty");
            
            switch (_currentState)
            {
                case States.WaitingForGridData:
                    ParseGridData(input, out _grid);
                    _currentState = States.WaitingForRobotData1;
                    break;
                case States.WaitingForRobotData1:
                    if (CheckForOk(input))
                    {
                        _currentState = States.Ready;
                        return ParseInstruction(input, out error);
                    }

                    if (_grid == null)
                        throw new ApplicationException("Grid is null when it shouldn't be");

                    ParseLocationAndDirectionData(input, _grid, out var location, out var direction);
                    _currentState = States.WaitingForRobotData2;
                    _robots.Add(new Robot(location, direction));
                    break;
                case States.WaitingForRobotData2:
                    ParseCommandData(input, out var commands);
                    _currentState = States.WaitingForRobotData1;
                    _robots.Last().SetCommands(commands);
                    break;
                case States.Ready:
                    _okAccepted = true;
                    Run();
                    foreach (var robot in _robots)
                    {
                        var robotData = string.Join(" ",
                            robot.CurrentPosition.X, robot.CurrentPosition.Y,
                            (char)robot.CurrentDirection, robot.IsLost ? "LOST" : string.Empty);
                        _ioProvider.WriteLine($"Robot {robot.Id} last position: {robotData}");
                    }
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
    
    private void ProcessInput(string message)
    {
        bool ok;
        do
        {
            _ioProvider.WriteLine(message);
            _ioProvider.Write("> ");
            ok = ParseInstruction(_ioProvider.ReadLine(), out var error);
            if (ok) continue;
            _ioProvider.WriteLine($"Incorrect format. {error}");
        } while (!ok);
    }

    public void ProcessInput()
    {
        ProcessInput("Please, input grid size (X and Y) and press Enter. For example '5 3'.");
        Console.WriteLine("Grid created. Thank you. ");

        while (!_okAccepted)
        {
            ProcessInput(
                "Please, input robot's position (integer X and Y) and direction (one of chars: N/S/W/E) in bounds of previously entered grid size. For example. '1 1 E'");
            if (_okAccepted) break;
            ProcessInput(
                "Please, input robot's commands (sequence of chars: L/R/F) where L is Turn Left, R is Turn Right, F is Move Forward. For example. 'RFFLRF'");
            Console.WriteLine($"Robot (ID: {_robots.Last().Id}) was created.");
        } 
    }

    #endregion

    #region IDisposable

    public void Dispose()
    {
        _robots.Clear();
    }
    
    #endregion
}