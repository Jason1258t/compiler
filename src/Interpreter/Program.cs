namespace Interpreter;

/// <summary>
///  Точка входа консольного интерпретатора: <c>dotscript &lt;путь к файлу&gt;</c>.
/// </summary>
public static class Program
{
    private const int ErrorExitCode = 1;

    public static int Main(string[] args)
    {
        ConsoleEnvironment environment = new();
        if (args.Length != 1)
        {
            environment.WriteError("Использование: dotscript <путь к файлу>");
            return ErrorExitCode;
        }

        string sourceCode;
        try
        {
            sourceCode = File.ReadAllText(args[0]);
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
        {
            environment.WriteError($"Не удалось прочитать файл '{args[0]}': {exception.Message}");
            return ErrorExitCode;
        }

        DotScriptInterpreter interpreter = new(environment);
        return interpreter.Run(sourceCode) ? 0 : ErrorExitCode;
    }
}