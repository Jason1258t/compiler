namespace Parsing;

// Стандартные конструкторы не нужны: ошибка всегда содержит позицию в исходном коде.
#pragma warning disable RCS1194

/// <summary>
///  Ошибка синтаксического анализа.
/// </summary>
public class SyntaxException : Exception
{
    public SyntaxException(string message, int line, int column)
        : base($"Синтаксическая ошибка в строке {line}, столбце {column}: {message}")
    {
        Line = line;
        Column = column;
    }

    /// <summary>
    ///  Номер строки, в которой обнаружена ошибка (начиная с 1).
    /// </summary>
    public int Line { get; }

    /// <summary>
    ///  Номер столбца, в котором обнаружена ошибка (начиная с 1).
    /// </summary>
    public int Column { get; }
}
#pragma warning restore RCS1194