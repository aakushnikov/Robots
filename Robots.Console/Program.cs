using Robots.Middleware;

using (var processor = new Processor(new ConsoleIoProvider()))
    processor.ProcessInput();

Console.WriteLine("Press any key to exit...");
Console.ReadKey();