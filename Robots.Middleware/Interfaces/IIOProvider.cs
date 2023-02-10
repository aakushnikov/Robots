namespace Robots.Middleware.Interfaces;

public interface IIOProvider
{
    string? ReadLine();
    void WriteLine(bool isInfoMessage, string? s = null);
    void Write(string? s = null);
    bool AllowInfoMessages { get; }
}