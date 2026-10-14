using Xunit;

namespace Lexing.UnitTests;

public class TokenValueTest
{
    [Fact]
    public void Can_convert_integer_value_to_int()
    {
        TokenValue value = new(42);
        Assert.Equal(42, value.ToInt());
    }

    [Fact]
    public void Can_convert_integer_value_to_string()
    {
        TokenValue value = new(2147483647);
        Assert.Equal("2147483647", value.ToString());
    }

    [Fact]
    public void Can_convert_string_value_to_string()
    {
        TokenValue value = new("text");
        Assert.Equal("text", value.ToString());
    }

    [Fact]
    public void Throws_when_converting_string_value_to_int()
    {
        TokenValue value = new("42");
        Assert.Throws<InvalidOperationException>(() => value.ToInt());
    }

    [Fact]
    public void Can_format_tokens_as_text()
    {
        List<string> actual =
        [
            new Token(TokenType.Semicolon).ToString(),
            new Token(TokenType.Identifier, new TokenValue("x")).ToString(),
            new Token(TokenType.IntegerLiteral, new TokenValue(5)).ToString(),
        ];
        Assert.Equal(["Semicolon", "Identifier (x)", "IntegerLiteral (5)"], actual);
    }
}