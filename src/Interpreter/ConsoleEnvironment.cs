using Execution;

namespace Interpreter;

/// <summary>
///  Окружение, связанное со стандартными потоками ввода, вывода и ошибок.
/// </summary>
public class ConsoleEnvironment : IEnvironment
{
    public string? ReadLine()
    {
        return Console.In.ReadLine();
    }

    public void Write(string text)
    {
        Console.Out.Write(text);
    }

    public void WriteError(string message)
    {
        Console.Error.WriteLine(message);
    }
}