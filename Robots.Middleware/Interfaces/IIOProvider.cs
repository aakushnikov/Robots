namespace Robots.Middleware.Interfaces;

public interface IIOProvider
{
    string? ReadLine();
    void WriteLine(string? s = null);
    void Write(string? s = null);
}