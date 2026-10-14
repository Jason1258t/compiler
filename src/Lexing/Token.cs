namespace Lexing;

/// <summary>
///  Лексема: тип и, для идентификаторов и литералов, значение.
/// </summary>
public sealed record Token(TokenType Type, TokenValue? Value = null)
{
    public override string ToString()
    {
        return Value is null ? Type.ToString() : $"{Type} ({Value})";
    }
}