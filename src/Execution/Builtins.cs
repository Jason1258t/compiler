using System.Globalization;

namespace Execution;

/// <summary>
///  Встроенные функции языка DotScript.
/// </summary>
public class Builtins
{
    private readonly IEnvironment environment;
    private readonly Dictionary<string, BuiltinFunction> functions;

    public Builtins(IEnvironment environment)
    {
        this.environment = environment;
        functions = new Dictionary<string, BuiltinFunction>
        {
            { "print", new BuiltinFunction(1, Print) },
            { "println", new BuiltinFunction(1, PrintLine) },
            { "readInt", new BuiltinFunction(0, ReadInt) },
            { "abs", new BuiltinFunction(1, arguments => Abs(arguments[0])) },
            { "min", new BuiltinFunction(2, arguments => Math.Min(arguments[0], arguments[1])) },
            { "max", new BuiltinFunction(2, arguments => Math.Max(arguments[0], arguments[1])) },
        };
    }

    /// <summary>
    ///  Вызывает встроенную функцию.
    ///  Возвращает результат функции либо <c>null</c>, если функция не возвращает значения.
    /// </summary>
    /// <exception cref="RuntimeException">Если функция неизвестна, число аргументов неверно или вызов завершился ошибкой.</exception>
    public int? Call(string name, IReadOnlyList<int> arguments)
    {
        if (!functions.TryGetValue(name, out BuiltinFunction? function))
        {
            throw new RuntimeException($"неизвестная функция '{name}'");
        }

        if (arguments.Count != function.ParameterCount)
        {
            throw new RuntimeException(
                $"функция '{name}' ожидает аргументов: {function.ParameterCount}, передано: {arguments.Count}"
            );
        }

        return function.Body(arguments);
    }

    private static int Abs(int value)
    {
        return value == int.MinValue ? throw new RuntimeException("переполнение целого числа") : Math.Abs(value);
    }

    private static string Format(int value)
    {
        return value.ToString(CultureInfo.InvariantCulture);
    }

    private int? Print(IReadOnlyList<int> arguments)
    {
        environment.Write(Format(arguments[0]));
        return null;
    }

    private int? PrintLine(IReadOnlyList<int> arguments)
    {
        environment.Write(Format(arguments[0]) + "\n");
        return null;
    }

    private int? ReadInt(IReadOnlyList<int> arguments)
    {
        string line = environment.ReadLine() ?? throw new RuntimeException("ввод закончился");
        if (!int.TryParse(line.Trim(), NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out int value))
        {
            throw new RuntimeException($"строка \"{line}\" не является целым числом");
        }

        return value;
    }

    /// <summary>
    ///  Встроенная функция: число параметров и реализация.
    /// </summary>
    private sealed record BuiltinFunction(int ParameterCount, Func<IReadOnlyList<int>, int?> Body);
}