using Xunit;

namespace Lexing.UnitTests;

public class TextScannerTest
{
    [Fact]
    public void Can_peek_ahead_without_advancing()
    {
        TextScanner scanner = new("abc");

        List<char> actual = [scanner.Peek(), scanner.Peek(1), scanner.Peek(2), scanner.Peek(3), scanner.Peek()];

        Assert.Equal(['a', 'b', 'c', TextScanner.EndOfText, 'a'], actual);
    }

    [Fact]
    public void Tracks_line_and_column_for_all_line_breaks()
    {
        TextScanner scanner = new("a\nb\r\nc\rd");
        List<(char Symbol, int Line, int Column)> expected =
        [
            ('a', 1, 1), ('\n', 1, 2), ('b', 2, 1), ('\r', 2, 2), ('\n', 2, 2), ('c', 3, 1), ('\r', 3, 2), ('d', 4, 1),
        ];

        List<(char Symbol, int Line, int Column)> actual = [];
        while (!scanner.IsEnd())
        {
            actual.Add((scanner.Peek(), scanner.Line, scanner.Column));
            scanner.Advance();
        }

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Does_not_move_past_end_of_text()
    {
        TextScanner scanner = new("a");

        scanner.Advance();
        scanner.Advance();

        Assert.True(scanner.IsEnd());
        Assert.Equal(TextScanner.EndOfText, scanner.Peek());
        Assert.Equal((1, 2), (scanner.Line, scanner.Column));
    }
}