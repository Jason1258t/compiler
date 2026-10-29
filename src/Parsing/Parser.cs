using Execution;

using Lexing;

namespace Parsing;

/// <summary>
///  Синтаксический анализатор языка DotScript, работающий методом рекурсивного спуска.
///  Выполняет программу по мере разбора: каждый метод разбора правила грамматики
///  возвращает значение разобранного выражения.
/// </summary>
/// <remarks>
///  Значение <c>null</c> означает, что выражение не возвращает значения, например вызов <c>print</c>.
/// </remarks>
public class Parser
{
    private readonly Lexer lexer;
    private readonly Builtins builtins;
    private Token current;

    public Parser(string sourceCode, Builtins builtins)
    {
        lexer = new Lexer(sourceCode);
        this.builtins = builtins;
        current = new Token(TokenType.EndOfFile);
    }

    /// <summary>
    ///  Разбирает и выполняет программу.
    ///  program = { statement } ;
    /// </summary>
    /// <exception cref="LexicalException">При лексической ошибке.</exception>
    /// <exception cref="SyntaxException">При синтаксической ошибке.</exception>
    /// <exception cref="RuntimeException">При ошибке выполнения.</exception>
    public void ParseProgram()
    {
        Advance();
        while (current.Type != TokenType.EndOfFile)
        {
            ParseStatement();
        }
    }

    /// <summary>
    ///  statement = expression , ";" ;
    /// </summary>
    private void ParseStatement()
    {
        ParseExpression();
        Expect(TokenType.Semicolon, ";");
    }

    /// <summary>
    ///  expression = additive_expression ;
    /// </summary>
    private int? ParseExpression()
    {
        return ParseAdditiveExpression();
    }

    /// <summary>
    ///  additive_expression = multiplicative_expression , { ( "+" | "-" ) , multiplicative_expression } ;
    /// </summary>
    private int? ParseAdditiveExpression()
    {
        int? result = ParseMultiplicativeExpression();
        while (current.Type is TokenType.Plus or TokenType.Minus)
        {
            int left = RequireValue(result);
            TokenType operation = current.Type;
            Advance();
            int right = RequireValue(ParseMultiplicativeExpression());

            result = operation == TokenType.Plus
                ? Arithmetic.Add(left, right)
                : Arithmetic.Subtract(left, right);
        }

        return result;
    }

    /// <summary>
    ///  multiplicative_expression = unary_expression , { ( "*" | "/" | "%" ) , unary_expression } ;
    /// </summary>
    private int? ParseMultiplicativeExpression()
    {
        int? result = ParseUnaryExpression();
        while (current.Type is TokenType.Multiply or TokenType.Divide or TokenType.Modulo)
        {
            int left = RequireValue(result);
            TokenType operation = current.Type;
            Advance();
            int right = RequireValue(ParseUnaryExpression());

            result = operation switch
            {
                TokenType.Multiply => Arithmetic.Multiply(left, right),
                TokenType.Divide => Arithmetic.Divide(left, right),
                _ => Arithmetic.Modulo(left, right),
            };
        }

        return result;
    }

    /// <summary>
    ///  unary_expression = "-" , unary_expression | primary_expression ;
    /// </summary>
    private int? ParseUnaryExpression()
    {
        if (current.Type == TokenType.Minus)
        {
            Advance();
            return Arithmetic.Negate(RequireValue(ParseUnaryExpression()));
        }

        return ParsePrimaryExpression();
    }

    /// <summary>
    ///  primary_expression = integer_literal | function_call | "(" , expression , ")" ;
    /// </summary>
    private int? ParsePrimaryExpression()
    {
        switch (current.Type)
        {
            case TokenType.IntegerLiteral:
                int value = current.Value!.ToInt();
                Advance();
                return value;

            case TokenType.Identifier:
                return ParseFunctionCall();

            case TokenType.OpenParenthesis:
                Advance();
                int? result = ParseExpression();
                Expect(TokenType.CloseParenthesis, ")");
                return result;

            default:
                throw new SyntaxException($"ожидалось выражение, получено {current}");
        }
    }

    /// <summary>
    ///  function_call = identifier , "(" , [ argument_list ] , ")" ;
    /// </summary>
    private int? ParseFunctionCall()
    {
        string name = current.Value!.ToString();
        Advance();
        Expect(TokenType.OpenParenthesis, "(");

        List<int> arguments = current.Type == TokenType.CloseParenthesis ? [] : ParseArgumentList();
        Expect(TokenType.CloseParenthesis, ")");

        return builtins.Call(name, arguments);
    }

    /// <summary>
    ///  argument_list = expression , { "," , expression } ;
    /// </summary>
    private List<int> ParseArgumentList()
    {
        List<int> arguments = [RequireValue(ParseExpression())];
        while (current.Type == TokenType.Comma)
        {
            Advance();
            arguments.Add(RequireValue(ParseExpression()));
        }

        return arguments;
    }

    /// <summary>
    ///  Проверяет, что выражение вернуло значение, которое можно использовать как операнд.
    /// </summary>
    private static int RequireValue(int? value)
    {
        return value ?? throw new RuntimeException("выражение не возвращает значения");
    }

    /// <summary>
    ///  Переходит к следующей лексеме.
    /// </summary>
    private void Advance()
    {
        current = lexer.ParseToken();
    }

    /// <summary>
    ///  Проверяет, что текущая лексема имеет ожидаемый тип, и переходит к следующей.
    /// </summary>
    private void Expect(TokenType type, string text)
    {
        if (current.Type != type)
        {
            throw new SyntaxException($"ожидалось '{text}', получено {current}");
        }

        Advance();
    }
}