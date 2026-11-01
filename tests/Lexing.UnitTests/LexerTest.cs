using Xunit;

using static Lexing.UnitTests.Helpers.Tokens;

namespace Lexing.UnitTests;

public class LexerTest
{
    [Theory]
    [MemberData(nameof(GetProgramsData))]
    public void Can_tokenize_programs(string code, List<Token> expected)
    {
        List<Token> actual = Lexer.Tokenize(code);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(GetKeywordsData))]
    public void Can_tokenize_keywords(string code, TokenType expected)
    {
        List<Token> actual = Lexer.Tokenize(code);

        Assert.Equal([Of(expected)], actual);
    }

    [Theory]
    [MemberData(nameof(GetOperatorsAndPunctuationData))]
    public void Can_tokenize_operators_and_punctuation(string code, TokenType expected)
    {
        List<Token> actual = Lexer.Tokenize(code);

        Assert.Equal([Of(expected)], actual);
    }

    [Theory]
    [MemberData(nameof(GetLongestMatchData))]
    public void Prefers_longest_lexeme(string code, List<Token> expected)
    {
        List<Token> actual = Lexer.Tokenize(code);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(GetIdentifiersData))]
    public void Can_tokenize_identifiers(string code, List<Token> expected)
    {
        List<Token> actual = Lexer.Tokenize(code);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(GetIntegerLiteralsData))]
    public void Can_tokenize_integer_literals(string code, List<Token> expected)
    {
        List<Token> actual = Lexer.Tokenize(code);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(GetStringLiteralsData))]
    public void Can_tokenize_string_literals(string code, List<Token> expected)
    {
        List<Token> actual = Lexer.Tokenize(code);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(GetWhitespacesAndCommentsData))]
    public void Skips_whitespaces_and_comments(string code, List<Token> expected)
    {
        List<Token> actual = Lexer.Tokenize(code);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Returns_end_of_file_repeatedly_after_end_of_text()
    {
        Lexer lexer = new("x");

        List<Token> actual = [lexer.ParseToken(), lexer.ParseToken(), lexer.ParseToken()];

        Assert.Equal([Identifier("x"), Of(TokenType.EndOfFile), Of(TokenType.EndOfFile)], actual);
    }

    [Theory]
    [MemberData(nameof(GetLexicalErrorsData))]
    public void Reports_lexical_errors(string code, int line, int column, string reason)
    {
        Exception? exception = Record.Exception(() => Lexer.Tokenize(code));

        LexicalException lexicalException = Assert.IsType<LexicalException>(exception);
        Assert.Equal((line, column), (lexicalException.Line, lexicalException.Column));
        Assert.Equal($"Лексическая ошибка в строке {line}, столбце {column}: {reason}", lexicalException.Message);
    }

    public static TheoryData<string, List<Token>> GetProgramsData()
    {
        return new TheoryData<string, List<Token>>
        {
            {
                "let x: int = 10;",
                [
                    Of(TokenType.Let), Identifier("x"), Of(TokenType.Colon), Of(TokenType.Int),
                    Of(TokenType.Assign), Integer(10), Of(TokenType.Semicolon),
                ]
            },
            {
                """
                // Сумма двух чисел.
                fn sum(a: int, b: int): int {
                    return a + b;
                }
                println(sum(1, 2));
                """,
                [
                    Of(TokenType.Fn), Identifier("sum"), Of(TokenType.OpenParenthesis),
                    Identifier("a"), Of(TokenType.Colon), Of(TokenType.Int), Of(TokenType.Comma),
                    Identifier("b"), Of(TokenType.Colon), Of(TokenType.Int), Of(TokenType.CloseParenthesis),
                    Of(TokenType.Colon), Of(TokenType.Int), Of(TokenType.OpenBrace),
                    Of(TokenType.Return), Identifier("a"), Of(TokenType.Plus), Identifier("b"), Of(TokenType.Semicolon),
                    Of(TokenType.CloseBrace),
                    Identifier("println"), Of(TokenType.OpenParenthesis), Identifier("sum"),
                    Of(TokenType.OpenParenthesis), Integer(1), Of(TokenType.Comma), Integer(2),
                    Of(TokenType.CloseParenthesis), Of(TokenType.CloseParenthesis), Of(TokenType.Semicolon),
                ]
            },
            {
                """
                struct Point { x: int; y: int; }
                let points: Point[] = [a, b];
                while (i < 3 && !done) { points[i].x = i % 2; }
                if (s != "end") { print(s); } else { println("bye"); }
                """,
                [
                    Of(TokenType.Struct), Identifier("Point"), Of(TokenType.OpenBrace),
                    Identifier("x"), Of(TokenType.Colon), Of(TokenType.Int), Of(TokenType.Semicolon),
                    Identifier("y"), Of(TokenType.Colon), Of(TokenType.Int), Of(TokenType.Semicolon),
                    Of(TokenType.CloseBrace),
                    Of(TokenType.Let), Identifier("points"), Of(TokenType.Colon), Identifier("Point"),
                    Of(TokenType.OpenBracket), Of(TokenType.CloseBracket), Of(TokenType.Assign), Of(TokenType.OpenBracket),
                    Identifier("a"), Of(TokenType.Comma), Identifier("b"), Of(TokenType.CloseBracket),
                    Of(TokenType.Semicolon),
                    Of(TokenType.While), Of(TokenType.OpenParenthesis), Identifier("i"), Of(TokenType.Less), Integer(3),
                    Of(TokenType.And), Of(TokenType.Not), Identifier("done"), Of(TokenType.CloseParenthesis),
                    Of(TokenType.OpenBrace), Identifier("points"), Of(TokenType.OpenBracket), Identifier("i"),
                    Of(TokenType.CloseBracket), Of(TokenType.Dot), Identifier("x"), Of(TokenType.Assign),
                    Identifier("i"), Of(TokenType.Modulo), Integer(2), Of(TokenType.Semicolon), Of(TokenType.CloseBrace),
                    Of(TokenType.If), Of(TokenType.OpenParenthesis), Identifier("s"), Of(TokenType.NotEqual),
                    String("end"), Of(TokenType.CloseParenthesis), Of(TokenType.OpenBrace), Identifier("print"),
                    Of(TokenType.OpenParenthesis), Identifier("s"), Of(TokenType.CloseParenthesis),
                    Of(TokenType.Semicolon), Of(TokenType.CloseBrace), Of(TokenType.Else), Of(TokenType.OpenBrace),
                    Identifier("println"), Of(TokenType.OpenParenthesis), String("bye"), Of(TokenType.CloseParenthesis),
                    Of(TokenType.Semicolon), Of(TokenType.CloseBrace),
                ]
            },
            {
                "let ok: bool = a >= b || c <= d == true != false;",
                [
                    Of(TokenType.Let), Identifier("ok"), Of(TokenType.Colon), Of(TokenType.Bool), Of(TokenType.Assign),
                    Identifier("a"), Of(TokenType.GreaterOrEqual), Identifier("b"), Of(TokenType.Or),
                    Identifier("c"), Of(TokenType.LessOrEqual), Identifier("d"), Of(TokenType.Equal),
                    Of(TokenType.True), Of(TokenType.NotEqual), Of(TokenType.False), Of(TokenType.Semicolon),
                ]
            },
            {
                "let name: str = readStr(); let n: int = -readInt() * 2 / 3;",
                [
                    Of(TokenType.Let), Identifier("name"), Of(TokenType.Colon), Of(TokenType.Str),
                    Of(TokenType.Assign), Identifier("readStr"), Of(TokenType.OpenParenthesis),
                    Of(TokenType.CloseParenthesis), Of(TokenType.Semicolon),
                    Of(TokenType.Let), Identifier("n"), Of(TokenType.Colon), Of(TokenType.Int), Of(TokenType.Assign),
                    Of(TokenType.Minus), Identifier("readInt"), Of(TokenType.OpenParenthesis),
                    Of(TokenType.CloseParenthesis), Of(TokenType.Multiply), Integer(2), Of(TokenType.Divide),
                    Integer(3), Of(TokenType.Semicolon),
                ]
            },
            { string.Empty, [] },
        };
    }

    public static TheoryData<string, TokenType> GetKeywordsData()
    {
        return new TheoryData<string, TokenType>
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
    }

    public static TheoryData<string, TokenType> GetOperatorsAndPunctuationData()
    {
        return new TheoryData<string, TokenType>
        {
            { "=", TokenType.Assign },
            { "==", TokenType.Equal },
            { "!=", TokenType.NotEqual },
            { "<", TokenType.Less },
            { "<=", TokenType.LessOrEqual },
            { ">", TokenType.Greater },
            { ">=", TokenType.GreaterOrEqual },
            { "&&", TokenType.And },
            { "||", TokenType.Or },
            { "!", TokenType.Not },
            { "+", TokenType.Plus },
            { "-", TokenType.Minus },
            { "*", TokenType.Multiply },
            { "/", TokenType.Divide },
            { "%", TokenType.Modulo },
            { ".", TokenType.Dot },
            { "[", TokenType.OpenBracket },
            { "]", TokenType.CloseBracket },
            { "(", TokenType.OpenParenthesis },
            { ")", TokenType.CloseParenthesis },
            { "{", TokenType.OpenBrace },
            { "}", TokenType.CloseBrace },
            { ",", TokenType.Comma },
            { ";", TokenType.Semicolon },
            { ":", TokenType.Colon },
        };
    }

    public static TheoryData<string, List<Token>> GetLongestMatchData()
    {
        return new TheoryData<string, List<Token>>
        {
            { "===", [Of(TokenType.Equal), Of(TokenType.Assign)] },
            { "!==", [Of(TokenType.NotEqual), Of(TokenType.Assign)] },
            { "!!", [Of(TokenType.Not), Of(TokenType.Not)] },
            { "<<=", [Of(TokenType.Less), Of(TokenType.LessOrEqual)] },
            { "< =", [Of(TokenType.Less), Of(TokenType.Assign)] },
            { ">>=", [Of(TokenType.Greater), Of(TokenType.GreaterOrEqual)] },
            { "&&&&", [Of(TokenType.And), Of(TokenType.And)] },
            { "||||", [Of(TokenType.Or), Of(TokenType.Or)] },
            { "a.b.c", [Identifier("a"), Of(TokenType.Dot), Identifier("b"), Of(TokenType.Dot), Identifier("c")] },
            { "x--1", [Identifier("x"), Of(TokenType.Minus), Of(TokenType.Minus), Integer(1)] },
            { "1.5", [Integer(1), Of(TokenType.Dot), Integer(5)] },
            { "a/b", [Identifier("a"), Of(TokenType.Divide), Identifier("b")] },
        };
    }

    public static TheoryData<string, List<Token>> GetIdentifiersData()
    {
        return new TheoryData<string, List<Token>>
        {
            { "x", [Identifier("x")] },
            { "value", [Identifier("value")] },
            { "camelCase", [Identifier("camelCase")] },
            { "PascalCase", [Identifier("PascalCase")] },
            { "snake_case", [Identifier("snake_case")] },
            { "_", [Identifier("_")] },
            { "__private", [Identifier("__private")] },
            { "trailing_", [Identifier("trailing_")] },
            { "x1y2z3", [Identifier("x1y2z3")] },
            { "_0", [Identifier("_0")] },
            { "ABCDEFGHIJKLMNOPQRSTUVWXYZ", [Identifier("ABCDEFGHIJKLMNOPQRSTUVWXYZ")] },
            { "abcdefghijklmnopqrstuvwxyz_0123456789", [Identifier("abcdefghijklmnopqrstuvwxyz_0123456789")] },

            // Регистр имеет значение.
            { "value Value VALUE", [Identifier("value"), Identifier("Value"), Identifier("VALUE")] },
            { "While", [Identifier("While")] },
            { "TRUE", [Identifier("TRUE")] },
            { "Int", [Identifier("Int")] },

            // Ключевое слово как часть идентификатора.
            { "iffy", [Identifier("iffy")] },
            { "letter", [Identifier("letter")] },
            { "int32", [Identifier("int32")] },
            { "true_", [Identifier("true_")] },
            { "_while", [Identifier("_while")] },
            { "fnfn", [Identifier("fnfn")] },

            // Имена встроенных функций — обычные идентификаторы.
            { "print", [Identifier("print")] },
            { "println", [Identifier("println")] },
            { "readInt", [Identifier("readInt")] },
            { "readStr", [Identifier("readStr")] },

            // Идентификатор не начинается с цифры.
            { "1abc", [Integer(1), Identifier("abc")] },
            { "007bond", [Integer(7), Identifier("bond")] },

            // Идентификатор завершается на первом символе, не входящем в его алфавит.
            { "a+b", [Identifier("a"), Of(TokenType.Plus), Identifier("b")] },
            { "f(x)", [Identifier("f"), Of(TokenType.OpenParenthesis), Identifier("x"), Of(TokenType.CloseParenthesis)] },
            { "a\"s\"", [Identifier("a"), String("s")] },
        };
    }

    public static TheoryData<string, List<Token>> GetIntegerLiteralsData()
    {
        return new TheoryData<string, List<Token>>
        {
            { "0", [Integer(0)] },
            { "7", [Integer(7)] },
            { "42", [Integer(42)] },
            { "1234567890", [Integer(1234567890)] },

            // Ведущие нули не влияют на значение.
            { "007", [Integer(7)] },
            { "000", [Integer(0)] },
            { "0000000000002147483647", [Integer(2147483647)] },

            // Максимальное значение.
            { "2147483647", [Integer(int.MaxValue)] },

            // Знак не входит в литерал.
            { "-12", [Of(TokenType.Minus), Integer(12)] },
            { "+12", [Of(TokenType.Plus), Integer(12)] },
            { "-2147483647", [Of(TokenType.Minus), Integer(int.MaxValue)] },

            // Несколько литералов подряд.
            { "1 2 3", [Integer(1), Integer(2), Integer(3)] },
            { "1;", [Integer(1), Of(TokenType.Semicolon)] },
        };
    }

    public static TheoryData<string, List<Token>> GetStringLiteralsData()
    {
        return new TheoryData<string, List<Token>>
        {
            { "\"hello\"", [String("hello")] },
            { "\"\"", [String(string.Empty)] },
            { "\"Hello, world!\"", [String("Hello, world!")] },
            { "\"Привет, мир! 你好 🙂\"", [String("Привет, мир! 你好 🙂")] },
            { "\"0123456789 @#$&|'`~^?\"", [String("0123456789 @#$&|'`~^?")] },

            // Escape-последовательности.
            { "\"\\n\"", [String("\n")] },
            { "\"\\r\"", [String("\r")] },
            { "\"\\t\"", [String("\t")] },
            { "\"\\\"\"", [String("\"")] },
            { "\"\\\\\"", [String("\\")] },
            { "\"line1\\r\\nline2\"", [String("line1\r\nline2")] },
            { "\"say \\\"hi\\\"\"", [String("say \"hi\"")] },
            { "\"C:\\\\dir\\\\file\"", [String("C:\\dir\\file")] },
            { "\"\\\\n\"", [String("\\n")] },
            { "\"\\n\\r\\t\\\"\\\\\"", [String("\n\r\t\"\\")] },

            // Пробелы, табуляция и комментарии внутри строки являются её частью.
            { "\"  a  b  \"", [String("  a  b  ")] },
            { "\"\ta\t\"", [String("\ta\t")] },
            { "\"// not a comment\"", [String("// not a comment")] },
            { "\"/* not a comment */\"", [String("/* not a comment */")] },
            { "\"*/\"", [String("*/")] },

            // Ключевые слова и операторы внутри строки являются её частью.
            { "\"let x = 1;\"", [String("let x = 1;")] },

            // Несколько строк подряд.
            { "\"a\"\"b\"", [String("a"), String("b")] },
            { "\"a\" + \"b\"", [String("a"), Of(TokenType.Plus), String("b")] },
        };
    }

    public static TheoryData<string, List<Token>> GetWhitespacesAndCommentsData()
    {
        return new TheoryData<string, List<Token>>
        {
            // Пробельные символы.
            { "a b", [Identifier("a"), Identifier("b")] },
            { "a\tb", [Identifier("a"), Identifier("b")] },
            { "a\nb", [Identifier("a"), Identifier("b")] },
            { "a\rb", [Identifier("a"), Identifier("b")] },
            { "a\r\nb", [Identifier("a"), Identifier("b")] },
            { " \t\r\n a \t\r\n ", [Identifier("a")] },
            { " \t\r\n ", [] },

            // Однострочные комментарии.
            { "// comment", [] },
            { "//", [] },
            { "a // comment", [Identifier("a")] },
            { "a // comment\nb", [Identifier("a"), Identifier("b")] },
            { "a // comment\rb", [Identifier("a"), Identifier("b")] },
            { "a // comment\r\nb", [Identifier("a"), Identifier("b")] },
            { "a//b", [Identifier("a")] },
            { "// a /* b\nc", [Identifier("c")] },
            { "// \"unclosed\nc", [Identifier("c")] },
            { "///", [] },
            { "// 1\n// 2\n// 3\nx", [Identifier("x")] },

            // Многострочные комментарии.
            { "/* comment */", [] },
            { "/**/", [] },
            { "/***/", [] },
            { "/* * / */", [] },
            { "a/* comment */b", [Identifier("a"), Identifier("b")] },
            { "a /* line1\nline2\r\nline3\r */ b", [Identifier("a"), Identifier("b")] },
            { "/* // */ x", [Identifier("x")] },
            { "/* \"unclosed */ x", [Identifier("x")] },

            // Вложенные комментарии не поддерживаются: комментарий завершается на ближайшем "*/".
            { "/* /* inner */ x", [Identifier("x")] },
            { "/* a */ */", [Of(TokenType.Multiply), Of(TokenType.Divide)] },

            // Комментарий без пробелов вокруг операторов.
            { "1/*x*/+/*y*/2", [Integer(1), Of(TokenType.Plus), Integer(2)] },
            { "a//*b*/\nc", [Identifier("a"), Identifier("c")] },
        };
    }

    public static TheoryData<string, int, int, string> GetLexicalErrorsData()
    {
        const string integerOverflow = "литерал целого числа превышает 2147483647";
        const string unclosedString = "литерал строки без закрывающей кавычки";
        const string invalidEscape = "недопустимая escape-последовательность";
        const string unclosedComment = "незавершённый многострочный комментарий";

        return new TheoryData<string, int, int, string>
        {
            // Символы, с которых не начинается ни одна лексема.
            { "@", 1, 1, "недопустимый символ '@'" },
            { "#", 1, 1, "недопустимый символ '#'" },
            { "$", 1, 1, "недопустимый символ '$'" },
            { "a ? b", 1, 3, "недопустимый символ '?'" },
            { "x\n  ~", 2, 3, "недопустимый символ '~'" },
            { "x\r\n^", 2, 1, "недопустимый символ '^'" },
            { "x\r\r'", 3, 1, "недопустимый символ '''" },
            { "\"a\" `", 1, 5, "недопустимый символ '`'" },

            // Буквы не из английского алфавита.
            { "переменная", 1, 1, "недопустимый символ 'п'" },
            { "abc€", 1, 4, "недопустимый символ '€'" },

            // Пробельные символы, не входящие в список допустимых.
            { "a\vb", 1, 2, "недопустимый символ '\v'" },
            { "a\fb", 1, 2, "недопустимый символ '\f'" },
            { "a\u00A0b", 1, 2, "недопустимый символ '\u00A0'" },

            // Одиночные & и |.
            { "&", 1, 1, "недопустимый символ '&'" },
            { "|", 1, 1, "недопустимый символ '|'" },
            { "a & b", 1, 3, "недопустимый символ '&'" },
            { "a | b", 1, 3, "недопустимый символ '|'" },
            { "&&&", 1, 3, "недопустимый символ '&'" },

            // Литерал целого числа больше 2147483647.
            { "2147483648", 1, 1, integerOverflow },
            { "x = 99999999999999999999;", 1, 5, integerOverflow },
            { "-2147483648", 1, 2, integerOverflow },

            // Литерал строки без закрывающей кавычки.
            { "\"", 1, 1, unclosedString },
            { "\"abc", 1, 1, unclosedString },
            { "x = \"abc\n\";", 1, 5, unclosedString },
            { "\"abc\r\"", 1, 1, unclosedString },
            { "\"abc\\\"", 1, 1, unclosedString },

            // Недопустимые escape-последовательности.
            { "\"\\a\"", 1, 2, invalidEscape },
            { "\"\\0\"", 1, 2, invalidEscape },
            { "\"\\'\"", 1, 2, invalidEscape },
            { "\"ok\\x\"", 1, 4, invalidEscape },
            { "\"\\N\"", 1, 2, invalidEscape },
            { "\"abc\\", 1, 5, invalidEscape },

            // Многострочный комментарий без завершающего "*/".
            { "/*", 1, 1, unclosedComment },
            { "/* comment", 1, 1, unclosedComment },
            { "x\n /* comment *", 2, 2, unclosedComment },
            { "/*/", 1, 1, unclosedComment },
        };
    }
}