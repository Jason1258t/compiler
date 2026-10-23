using Xunit;

namespace Lexing.UnitTests;

public class TokenTest
{
    [Theory]
    [MemberData(nameof(GetTokenTextsData))]
    public void Can_format_token_as_text(Token token, string expected)
    {
        string actual = token.ToString();

        Assert.Equal(expected, actual);
    }

    public static TheoryData<Token, string> GetTokenTextsData()
    {
        return new TheoryData<Token, string>
        {
            { new Token(TokenType.Semicolon), "Semicolon" },
            { new Token(TokenType.Identifier, new TokenValue("x")), "Identifier (x)" },
            { new Token(TokenType.IntegerLiteral, new TokenValue(5)), "IntegerLiteral (5)" },
            { new Token(TokenType.StringLiteral, new TokenValue("hi")), "StringLiteral (hi)" },
        };
    }
}