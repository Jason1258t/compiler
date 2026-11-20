namespace Lexing;

/// <summary>
///  Лексема: тип и, для идентификаторов и литералов, значение.
/// </summary>
/// <remarks>
///  Исходный текст и позиция лексемы не участвуют в сравнении:
///  лексемы равны, если совпадают их тип и значение.
/// </remarks>
public sealed record Token(TokenType Type, TokenValue? Value = null)
{
    /// <summary>
    ///  Текст лексемы в исходном коде; пустая строка для конца файла.
    /// </summary>
    public string Text { get; init; } = string.Empty;

    /// <summary>
    ///  Номер строки, с которой начинается лексема (начиная с 1).
    /// </summary>
    public int Line { get; init; }

    /// <summary>
    ///  Номер столбца, с которого начинается лексема (начиная с 1).
    /// </summary>
    public int Column { get; init; }

    public bool Equals(Token? other)
    {
        return other is not null && Type == other.Type && Equals(Value, other.Value);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Type, Value);
    }

    public override string ToString()
    {
        return Value is null ? Type.ToString() : $"{Type} ({Value})";
    }
}