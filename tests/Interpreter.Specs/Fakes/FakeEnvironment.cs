using System.Text;

using Execution;

namespace Interpreter.Specs.Fakes;

/// <summary>
///  Окружение для тестов: ввод задаётся заранее, вывод и ошибки накапливаются в памяти.
/// </summary>
public class FakeEnvironment : IEnvironment
{
    private readonly Queue<string> inputLines;
    private readonly StringBuilder output = new();
    private readonly List<string> errors = [];

    public FakeEnvironment(IEnumerable<string> inputLines)
    {
        this.inputLines = new Queue<string>(inputLines);
    }

    public string Output => output.ToString();

    public IReadOnlyList<string> Errors => errors;

    public string? ReadLine()
    {
        return inputLines.TryDequeue(out string? line) ? line : null;
    }

    public void Write(string text)
    {
        output.Append(text);
    }

    public void WriteError(string message)
    {
        errors.Add(message);
    }
}