using System.Reflection;
using Robots.Middleware;

namespace Robots.Test.Middleware;

[TestFixture]
public class ProcessorTest
{
    private static readonly object[] EmptyParameters = {};
    
    [Test]
    [TestCase("Run")]
    public void IsCannotRun_WithoutAcceptedInstructions(string methodName)
    {
        var processor = new Processor(new IoProviderMock());
        var method = processor.GetType().GetRuntimeMethods().FirstOrDefault(m => m.Name == methodName);
        Assert.IsNotNull(method);

        Assert.Throws<ApplicationException>(() =>
        {
            try
            {
                method?.Invoke(processor, EmptyParameters);

            }
            catch (Exception ex)
            {
                throw ex.InnerException!;
            }
        });
    }

    public static object[] InputData =
    {
        new [] { "3 3", "1 1 E", "FLFL", "OK", "2 2 W" },
        new [] { "3 3", "2 2 N", "F", "OK", "2 2 N LOST" },
    };

    [TestCaseSource(nameof(InputData))]
    [Test]
    public void IsProcessed_Well(string? gridInput, string? ldInput, string? commandsInput, string? ok, string output)
    {
        var io = new IoProviderMock(new []
        {
            gridInput, ldInput, commandsInput, ok
        });

        using var processor = new Processor(io);
        processor.ProcessInput();
        
        Assert.That(io.Outputs.Count().Equals(1));
        Assert.That(output.Equals(io.Outputs.First()!.Trim()));
    }

    [Test]
    [TestCase("", "WaitingForGridData")]
    [TestCase("", "WaitingForRobotData1")]
    [TestCase("", "WaitingForRobotData2")]
    public void EmptyInstruction_WasNotParsed(string? input, string state)
    {
        const string methodName = "ParseInstruction";
        const string stateFieldName = "_currentState";
        
        var processor = new Processor(new IoProviderMock());
        
        var method = processor.GetType().GetRuntimeMethods().FirstOrDefault(m => m.Name == methodName);
        var field = processor.GetType().GetRuntimeFields().FirstOrDefault(f => f.Name == stateFieldName);
        
        Assert.IsNotNull(method);
        Assert.IsNotNull(field);
        
        field!.SetValue(processor, Enum.Parse<Processor.States>(state));
        var errors = string.Empty;
        var args = new object?[] { input, errors };
        var result = method!.Invoke(processor, args);

        // We ensure that errors was 'out' last parameter of invoked method then we should retrieve it.
        errors = args.Last()!.ToString();
        
        Assert.That(!string.IsNullOrEmpty(errors));
        Assert.That(result!.Equals(false));
    }
    
    [Test]
    [TestCase("44")]
    [TestCase("A")]
    [TestCase("5 9 5")]
    [TestCase("4 3 A")]
    public void InstructionForGrid_WasNotParsed(string? input)
    {
        const string state = "WaitingForGridData";
        const string methodName = "ParseInstruction";
        const string stateFieldName = "_currentState";
        
        var processor = new Processor(new IoProviderMock());
        
        var method = processor.GetType().GetRuntimeMethods().FirstOrDefault(m => m.Name == methodName);
        var field = processor.GetType().GetRuntimeFields().FirstOrDefault(f => f.Name == stateFieldName);
        
        Assert.IsNotNull(method);
        Assert.IsNotNull(field);
        
        field!.SetValue(processor, Enum.Parse<Processor.States>(state));
        var errors = string.Empty;
        var args = new object?[] { input, errors };
        var result = method!.Invoke(processor, args);

        // We ensure that errors was 'out' last parameter of invoked method then we should retrieve it.
        errors = args.Last()!.ToString();
        
        Assert.That(!string.IsNullOrEmpty(errors));
        Assert.That(result!.Equals(false));
        
        // TODO Wise versa for passed
    }
    
    [Test]
    [TestCase("4 4", "B C")]
    [TestCase("3 3", "5 5 E")]
    [TestCase("2 2", "1 1 A")]
    [TestCase("1 1", "1 1 E")]
    public void InstructionForRobot_LocationAndDirection_WasNotParsed(string gridInput, string? ldInput)
    {
        const string state = "WaitingForGridData";
        const string methodName = "ParseInstruction";
        const string stateFieldName = "_currentState";
        
        var processor = new Processor(new IoProviderMock());
        
        var method = processor.GetType().GetRuntimeMethods().FirstOrDefault(m => m.Name == methodName);
        var field = processor.GetType().GetRuntimeFields().FirstOrDefault(f => f.Name == stateFieldName);
        
        Assert.IsNotNull(method);
        Assert.IsNotNull(field);
        
        field!.SetValue(processor, Enum.Parse<Processor.States>(state));
        var errors = string.Empty;
        var args = new object?[] { gridInput, errors };
        var resultForGrid = method!.Invoke(processor, args);
        // We ensure that errors was 'out' last parameter of invoked method then we should retrieve it.
        errors = args.Last()!.ToString();
        
        Assert.That(string.IsNullOrEmpty(errors));
        Assert.That(resultForGrid!.Equals(true));
        
        errors = string.Empty;
        args = new object?[] { ldInput, errors };
        
        // We don't need to switch the state again because it should be updated after grid instructions was accepted.
        // TODO The test for it
        var resultForLd = method!.Invoke(processor, args);
        // We ensure that errors was 'out' last parameter of invoked method then we should retrieve it.
        errors = args.Last()!.ToString();
        
        Assert.That(!string.IsNullOrEmpty(errors));
        Assert.That(resultForLd!.Equals(false));
    }
    

    [Test]
    [TestCase("3 3", "3 3 E")]
    public void IsNotProcessed(string? gridInput, string? ldInput)
    {
        var io = new IoProviderMock(new []
        {
            gridInput, ldInput
        });
    }
    
    // Those tests was for example. We can design a lot more any time.
}