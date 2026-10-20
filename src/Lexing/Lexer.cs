using System.Text;

namespace Lexing;

/// <summary>
///  Лексический анализатор языка DotScript.
///  Читает исходный текст посимвольно и возвращает лексемы по одной.
/// </summary>
public class Lexer
{
    private static readonly Dictionary<string, TokenType> Keywords = new()
    {
        { "bool", TokenType.Bool },
        { "else", TokenType.Else },
        { "false", TokenType.False },
        { "fn", TokenType.Fn },
        { "if", TokenType.If },
        { "int", TokenType.Int },
        { "let", TokenType.Let },
        { "return", TokenType.Return },
        { "str", TokenType.Str },
        { "struct", TokenType.Struct },
        { "true", TokenType.True },
        { "while", TokenType.While },
    };

    private readonly TextScanner scanner;

    public Lexer(string sourceCode)
    {
        scanner = new TextScanner(sourceCode);
    }

    /// <summary>
    ///  Разбивает исходный код на лексемы. Лексема <see cref="TokenType.EndOfFile"/> не включается в результат.
    /// </summary>
    /// <exception cref="LexicalException">При обнаружении лексической ошибки.</exception>
    public static List<Token> Tokenize(string sourceCode)
    {
        Lexer lexer = new(sourceCode);
        List<Token> tokens = [];
        for (Token token = lexer.ParseToken(); token.Type != TokenType.EndOfFile; token = lexer.ParseToken())
        {
            tokens.Add(token);
        }

        return tokens;
    }

    /// <summary>
    ///  Читает следующую лексему, пропуская пробельные символы и комментарии.
    ///  Когда текст закончился, возвращает лексему <see cref="TokenType.EndOfFile"/>.
    /// </summary>
    /// <exception cref="LexicalException">При обнаружении лексической ошибки.</exception>
    public Token ParseToken()
    {
        SkipWhitespacesAndComments();

        if (scanner.IsEnd())
        {
            return new Token(TokenType.EndOfFile);
        }

        char c = scanner.Peek();
        if (IsIdentifierStart(c))
        {
            return ParseIdentifierOrKeyword();
        }

        if (char.IsAsciiDigit(c))
        {
            return ParseIntegerLiteral();
        }

        if (c == '"')
        {
            return ParseStringLiteral();
        }

        return ParseOperatorOrPunctuation();
    }

    private static bool IsIdentifierStart(char c)
    {
        return char.IsAsciiLetter(c) || c == '_';
    }

    private static bool IsIdentifierPart(char c)
    {
        return char.IsAsciiLetterOrDigit(c) || c == '_';
    }

    private static bool IsWhitespace(char c)
    {
        return c is ' ' or '\t' or '\n' or '\r';
    }

    /// <summary>
    ///  Пропускает пробельные символы и комментарии между лексемами.
    /// </summary>
    private void SkipWhitespacesAndComments()
    {
        while (true)
        {
            if (IsWhitespace(scanner.Peek()))
            {
                scanner.Advance();
            }
            else if (scanner.Peek() == '/' && scanner.Peek(1) == '/')
            {
                SkipSingleLineComment();
            }
            else if (scanner.Peek() == '/' && scanner.Peek(1) == '*')
            {
                SkipMultiLineComment();
            }
            else
            {
                return;
            }
        }
    }

    /// <summary>
    ///  Пропускает однострочный комментарий: от "//" до LF, CR или конца файла.
    /// </summary>
    private void SkipSingleLineComment()
    {
        scanner.Advance();
        scanner.Advance();
        while (!scanner.IsEnd() && scanner.Peek() != '\n' && scanner.Peek() != '\r')
        {
            scanner.Advance();
        }
    }

    /// <summary>
    ///  Пропускает многострочный комментарий: от "/*" до ближайшего "*/".
    /// </summary>
    private void SkipMultiLineComment()
    {
        int line = scanner.Line;
        int column = scanner.Column;

        scanner.Advance();
        scanner.Advance();
        while (!(scanner.Peek() == '*' && scanner.Peek(1) == '/'))
        {
            if (scanner.IsEnd())
            {
                throw new LexicalException("незавершённый многострочный комментарий", line, column);
            }

            scanner.Advance();
        }

        scanner.Advance();
        scanner.Advance();
    }

    /// <summary>
    ///  Распознаёт идентификатор или ключевое слово.
    /// </summary>
    private Token ParseIdentifierOrKeyword()
    {
        StringBuilder name = new();
        while (IsIdentifierPart(scanner.Peek()))
        {
            name.Append(scanner.Peek());
            scanner.Advance();
        }

        string value = name.ToString();
        if (Keywords.TryGetValue(value, out TokenType keyword))
        {
            return new Token(keyword);
        }

        return new Token(TokenType.Identifier, new TokenValue(value));
    }

    /// <summary>
    ///  Распознаёт литерал целого числа, проверяя, что значение не превышает <see cref="int.MaxValue"/>.
    /// </summary>
    private Token ParseIntegerLiteral()
    {
        int line = scanner.Line;
        int column = scanner.Column;

        long value = 0;
        bool isOverflow = false;
        while (char.IsAsciiDigit(scanner.Peek()))
        {
            if (!isOverflow)
            {
                value = (value * 10) + (scanner.Peek() - '0');
                isOverflow = value > int.MaxValue;
            }

            scanner.Advance();
        }

        if (isOverflow)
        {
            throw new LexicalException($"литерал целого числа превышает {int.MaxValue}", line, column);
        }

        return new Token(TokenType.IntegerLiteral, new TokenValue((int)value));
    }

    /// <summary>
    ///  Распознаёт литерал строки в двойных кавычках, обрабатывая escape-последовательности.
    /// </summary>
    private Token ParseStringLiteral()
    {
        int line = scanner.Line;
        int column = scanner.Column;

        scanner.Advance();
        StringBuilder contents = new();
        while (scanner.Peek() != '"')
        {
            if (scanner.IsEnd() || scanner.Peek() == '\n' || scanner.Peek() == '\r')
            {
                throw new LexicalException("литерал строки без закрывающей кавычки", line, column);
            }

            if (scanner.Peek() == '\\')
            {
                contents.Append(ParseEscapeSequence());
            }
            else
            {
                contents.Append(scanner.Peek());
                scanner.Advance();
            }
        }

        scanner.Advance();
        return new Token(TokenType.StringLiteral, new TokenValue(contents.ToString()));
    }

    /// <summary>
    ///  Распознаёт escape-последовательность внутри литерала строки и возвращает обозначаемый ею символ.
    /// </summary>
    private char ParseEscapeSequence()
    {
        int line = scanner.Line;
        int column = scanner.Column;

        scanner.Advance();
        char escaped = scanner.Peek() switch
        {
            'n' => '\n',
            'r' => '\r',
            't' => '\t',
            '"' => '"',
            '\\' => '\\',
            _ => throw new LexicalException("недопустимая escape-последовательность", line, column),
        };
        scanner.Advance();

        return escaped;
    }

    /// <summary>
    ///  Распознаёт оператор или разделитель. Из подходящих лексем выбирается самая длинная.
    /// </summary>
    private Token ParseOperatorOrPunctuation()
    {
        int line = scanner.Line;
        int column = scanner.Column;

        char c = scanner.Peek();
        char next = scanner.Peek(1);
        (TokenType type, int length) = c switch
        {
            '=' when next == '=' => (TokenType.Equal, 2),
            '=' => (TokenType.Assign, 1),
            '!' when next == '=' => (TokenType.NotEqual, 2),
            '!' => (TokenType.Not, 1),
            '<' when next == '=' => (TokenType.LessOrEqual, 2),
            '<' => (TokenType.Less, 1),
            '>' when next == '=' => (TokenType.GreaterOrEqual, 2),
            '>' => (TokenType.Greater, 1),
            '&' when next == '&' => (TokenType.And, 2),
            '|' when next == '|' => (TokenType.Or, 2),
            '+' => (TokenType.Plus, 1),
            '-' => (TokenType.Minus, 1),
            '*' => (TokenType.Multiply, 1),
            '/' => (TokenType.Divide, 1),
            '%' => (TokenType.Modulo, 1),
            '.' => (TokenType.Dot, 1),
            '[' => (TokenType.OpenBracket, 1),
            ']' => (TokenType.CloseBracket, 1),
            '(' => (TokenType.OpenParenthesis, 1),
            ')' => (TokenType.CloseParenthesis, 1),
            '{' => (TokenType.OpenBrace, 1),
            '}' => (TokenType.CloseBrace, 1),
            ',' => (TokenType.Comma, 1),
            ';' => (TokenType.Semicolon, 1),
            ':' => (TokenType.Colon, 1),
            _ => throw new LexicalException($"недопустимый символ '{c}'", line, column),
        };

        for (int i = 0; i < length; i++)
        {
            scanner.Advance();
        }

        return new Token(type);
    }
}