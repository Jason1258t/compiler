using Xunit;

namespace Interpreter.Specs;

/// <summary>
///  Тесты точки входа консольного интерпретатора.
///  Подменяют стандартные потоки, поэтому выполняются последовательно в одной коллекции.
/// </summary>
[Collection(nameof(ProgramTest))]
public sealed class ProgramTest : IDisposable
{
    private readonly TextReader originalIn = Console.In;
    private readonly TextWriter originalOut = Console.Out;
    private readonly TextWriter originalError = Console.Error;
    private readonly StringWriter output = new();
    private readonly StringWriter error = new();
    private readonly string sourcePath = Path.GetTempFileName();

    public ProgramTest()
    {
        Console.SetOut(output);
        Console.SetError(error);
    }

    public void Dispose()
    {
        Console.SetIn(originalIn);
        Console.SetOut(originalOut);
        Console.SetError(originalError);
        output.Dispose();
        error.Dispose();
        File.Delete(sourcePath);
    }

    [Fact]
    public void Runs_program_from_file_with_console_input()
    {
        File.WriteAllText(sourcePath, "println(readInt() * 2);");
        Console.SetIn(new StringReader("21\n"));

        int exitCode = Program.Main([sourcePath]);

        Assert.Equal(0, exitCode);
        Assert.Equal("42\n", output.ToString());
        Assert.Equal(string.Empty, error.ToString());
    }

    [Fact]
    public void Reports_program_error_to_stderr()
    {
        File.WriteAllText(sourcePath, "println(1 / 0);");

        int exitCode = Program.Main([sourcePath]);

        Assert.Equal(1, exitCode);
        Assert.Equal(string.Empty, output.ToString());
        Assert.Equal("Ошибка выполнения: деление на ноль" + Environment.NewLine, error.ToString());
    }

    [Fact]
    public void Requires_exactly_one_argument()
    {
        int exitCode = Program.Main([]);

        Assert.Equal(1, exitCode);
        Assert.StartsWith("Использование: dotscript", error.ToString(), StringComparison.Ordinal);
    }

    [Fact]
    public void Reports_unreadable_file()
    {
        string missingPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString(), "missing.ds");

        int exitCode = Program.Main([missingPath]);

        Assert.Equal(1, exitCode);
        Assert.StartsWith($"Не удалось прочитать файл '{missingPath}'", error.ToString(), StringComparison.Ordinal);
    }
}