using Robots.Middleware.Interfaces;

namespace Robots.Middleware;

public sealed class ConsoleIoProvider : IIOProvider
{
    public bool AllowInfoMessages => true;

    public string? ReadLine()
    {
        return Console.ReadLine();
    }

    public void WriteLine(bool isInfoMessage, string? s = null)
    {
        if (isInfoMessage && !AllowInfoMessages) return;
        Console.WriteLine(s);
    }

    public void Write(string? s = null)
    {
        Console.Write(s);
    }
}