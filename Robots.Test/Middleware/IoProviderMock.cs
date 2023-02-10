using Robots.Middleware.Interfaces;

namespace Robots.Test.Middleware;

public class IoProviderMock : IIOProvider
{
    private readonly string?[] _inputs;
    private readonly IList<string?> _outputs;
    private int _index;

    public IEnumerable<string?> Outputs => _outputs;
    public bool AllowInfoMessages => false;

    public IoProviderMock() : this(new string?[] {}) { }

    public IoProviderMock(string?[] inputs)
    {
        _inputs = inputs;
        _outputs = new List<string?>();
    }
    
    public string? ReadLine()
    {
        return _inputs[_index++];
    }

    public void WriteLine(bool isInfoMessage, string? s = null)
    {
        if (isInfoMessage && !AllowInfoMessages) return;
        _outputs.Add(s);
    }

    public void Write(string? s = null)
    {
        // Do nothing cause of serviced behaviour of this method.
    }
}