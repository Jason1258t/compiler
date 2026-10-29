namespace Parsing;

// Стандартные конструкторы не нужны: ошибка всегда содержит описание причины.
#pragma warning disable RCS1194

/// <summary>
///  Ошибка синтаксического анализа.
/// </summary>
public class SyntaxException : Exception
{
    public SyntaxException(string message)
        : base($"Синтаксическая ошибка: {message}")
    {
    }
}
#pragma warning restore RCS1194