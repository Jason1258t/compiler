namespace Lexing.UnitTests.Helpers;

/// <summary>
///  Короткие фабричные методы для описания ожидаемых лексем в тестах.
/// </summary>
public static class Tokens
{
    public static Token Of(TokenType type)
    {
        return new Token(type);
    }

    public static Token Identifier(string name)
    {
        return new Token(TokenType.Identifier, new TokenValue(name));
    }

    public static Token Integer(int value)
    {
        return new Token(TokenType.IntegerLiteral, new TokenValue(value));
    }

    public static Token String(string value)
    {
        return new Token(TokenType.StringLiteral, new TokenValue(value));
    }
}