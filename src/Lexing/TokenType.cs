namespace Lexing;

/// <summary>
///  Типы лексем языка DotScript.
/// </summary>
public enum TokenType
{
    /// <summary>
    ///  Конец исходного кода.
    /// </summary>
    EndOfFile,

    /// <summary>
    ///  Идентификатор, например <c>value</c>.
    /// </summary>
    Identifier,

    /// <summary>
    ///  Литерал целого числа, например <c>42</c>.
    /// </summary>
    IntegerLiteral,

    /// <summary>
    ///  Литерал строки, например <c>"text"</c>.
    /// </summary>
    StringLiteral,

    // Ключевые слова.
    Bool,
    Else,
    False,
    Fn,
    If,
    Int,
    Let,
    New,
    Return,
    Str,
    Struct,
    True,
    While,

    // Операторы выражений.

    /// <summary>
    ///  Оператор присваивания <c>=</c>.
    /// </summary>
    Assign,

    /// <summary>
    ///  Оператор <c>==</c>.
    /// </summary>
    Equal,

    /// <summary>
    ///  Оператор <c>!=</c>.
    /// </summary>
    NotEqual,

    /// <summary>
    ///  Оператор <c>&lt;</c>.
    /// </summary>
    Less,

    /// <summary>
    ///  Оператор <c>&lt;=</c>.
    /// </summary>
    LessOrEqual,

    /// <summary>
    ///  Оператор <c>&gt;</c>.
    /// </summary>
    Greater,

    /// <summary>
    ///  Оператор <c>&gt;=</c>.
    /// </summary>
    GreaterOrEqual,

    /// <summary>
    ///  Оператор <c>&amp;&amp;</c>.
    /// </summary>
    And,

    /// <summary>
    ///  Оператор <c>||</c>.
    /// </summary>
    Or,

    /// <summary>
    ///  Оператор <c>!</c>.
    /// </summary>
    Not,

    /// <summary>
    ///  Оператор <c>+</c>.
    /// </summary>
    Plus,

    /// <summary>
    ///  Оператор <c>-</c>.
    /// </summary>
    Minus,

    /// <summary>
    ///  Оператор <c>*</c>.
    /// </summary>
    Multiply,

    /// <summary>
    ///  Оператор <c>/</c>.
    /// </summary>
    Divide,

    /// <summary>
    ///  Оператор <c>%</c>.
    /// </summary>
    Modulo,

    /// <summary>
    ///  Оператор доступа к полю <c>.</c>.
    /// </summary>
    Dot,

    // Прочие лексемы.

    /// <summary>
    ///  Символ <c>[</c>.
    /// </summary>
    OpenBracket,

    /// <summary>
    ///  Символ <c>]</c>.
    /// </summary>
    CloseBracket,

    /// <summary>
    ///  Символ <c>(</c>.
    /// </summary>
    OpenParenthesis,

    /// <summary>
    ///  Символ <c>)</c>.
    /// </summary>
    CloseParenthesis,

    /// <summary>
    ///  Символ <c>{</c>.
    /// </summary>
    OpenBrace,

    /// <summary>
    ///  Символ <c>}</c>.
    /// </summary>
    CloseBrace,

    /// <summary>
    ///  Символ <c>,</c>.
    /// </summary>
    Comma,

    /// <summary>
    ///  Символ <c>;</c>.
    /// </summary>
    Semicolon,

    /// <summary>
    ///  Символ <c>:</c>.
    /// </summary>
    Colon,
}