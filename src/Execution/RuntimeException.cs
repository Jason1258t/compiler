namespace Execution;

// Стандартные конструкторы не нужны: ошибка всегда содержит описание причины.
#pragma warning disable RCS1194

/// <summary>
///  Ошибка выполнения программы.
/// </summary>
public class RuntimeException : Exception
{
    public RuntimeException(string message)
        : base($"Ошибка выполнения: {message}")
    {
    }
}
#pragma warning restore RCS1194