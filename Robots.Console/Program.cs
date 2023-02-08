using Robots.Middleware;

var processor = new Processor();

ProcessInput("Please, input grid size (X and Y) and press Enter. For example '5 3'.", processor).Wait();
Console.WriteLine("Grid created. Thank you. ");

while (!processor.OkAccepted)
{
    ProcessInput(
        "Please, input robot's position (integer X and Y) and direction (one of chars: N/S/W/E) in bounds of previously entered grid size. For example. '1 1 E'.",
        processor).Wait();
    ProcessInput(
        "Please, input robot's commands (sequence of chars: L/R/F) where L is Turn Left, R is Turn Right, F is Move Forward. For example. 'RFFLRF'.",
        processor).Wait();
    Console.WriteLine("Robot created.");
}

static Task ProcessInput(string message, Processor processor)
{
    var ok = false;
    do
    {
        Console.WriteLine(message);
        ok = processor.ParseInstruction(Console.ReadLine(), out var error);
        if (ok) continue;
        Console.WriteLine($"Incorrect format. {error}");
    } while (!ok);
    return Task.CompletedTask;
}