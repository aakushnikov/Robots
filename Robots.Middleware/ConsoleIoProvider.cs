using Robots.Middleware.Interfaces;

namespace Robots.Middleware;

public sealed class ConsoleIoProvider : IIOProvider
{
    public string? ReadLine()
    {
        return Console.ReadLine();
    }

    public void WriteLine(string? s = null)
    {
        Console.WriteLine(s);
    }

    public void Write(string? s = null)
    {
        Console.Write(s);
    }
}