using Xunit;

namespace Lexing.UnitTests;

public class TokenValueTest
{
    [Theory]
    [MemberData(nameof(GetIntegerValuesData))]
    public void Can_convert_integer_value_to_int(int expected)
    {
        TokenValue value = new(expected);

        int actual = value.ToInt();

        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(GetStringRepresentationsData))]
    public void Can_convert_value_to_string(TokenValue value, string expected)
    {
        string actual = value.ToString();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Throws_when_converting_string_value_to_int()
    {
        TokenValue value = new("42");

        Exception? exception = Record.Exception(() => value.ToInt());

        InvalidOperationException invalidOperation = Assert.IsType<InvalidOperationException>(exception);
        Assert.Equal("Значение \"42\" не является целым числом", invalidOperation.Message);
    }

    public static TheoryData<int> GetIntegerValuesData()
    {
        return new TheoryData<int>
        {
            0,
            42,
            int.MaxValue,
        };
    }

    public static TheoryData<TokenValue, string> GetStringRepresentationsData()
    {
        return new TheoryData<TokenValue, string>
        {
            { new TokenValue(0), "0" },
            { new TokenValue(2147483647), "2147483647" },
            { new TokenValue("text"), "text" },
            { new TokenValue(string.Empty), string.Empty },
        };
    }
}