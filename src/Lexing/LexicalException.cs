namespace Lexing;

// Стандартные конструкторы не нужны: ошибка всегда содержит позицию в исходном коде.
#pragma warning disable RCS1194

/// <summary>
///  Ошибка лексического анализа.
/// </summary>
public class LexicalException : Exception
{
    public LexicalException(string message, int line, int column)
        : base($"Лексическая ошибка в строке {line}, столбце {column}: {message}")
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