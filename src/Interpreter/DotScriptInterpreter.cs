using Execution;

using Lexing;

using Parsing;

namespace Interpreter;

/// <summary>
///  Интерпретатор языка DotScript: разбирает и выполняет программу в заданном окружении.
/// </summary>
public class DotScriptInterpreter
{
    private readonly IEnvironment environment;

    public DotScriptInterpreter(IEnvironment environment)
    {
        this.environment = environment;
    }

    /// <summary>
    ///  Выполняет программу. При ошибке выводит сообщение в поток ошибок окружения.
    /// </summary>
    /// <returns><c>true</c>, если программа выполнена без ошибок.</returns>
    public bool Run(string sourceCode)
    {
        try
        {
            Parser parser = new(sourceCode, new Builtins(environment));
            parser.ParseProgram();
            return true;
        }
        catch (Exception exception) when (exception is LexicalException or SyntaxException or RuntimeException)
        {
            environment.WriteError(exception.Message);
            return false;
        }
    }
}