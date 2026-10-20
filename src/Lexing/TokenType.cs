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
    ///  Идентификатор.
    /// </summary>
    Identifier,

    /// <summary>
    ///  Литерал целого числа.
    /// </summary>
    IntegerLiteral,

    /// <summary>
    ///  Литерал строки.
    /// </summary>
    StringLiteral,

    /// <summary>
    ///  Ключевое слово <c>bool</c> — логический тип.
    /// </summary>
    Bool,

    /// <summary>
    ///  Ключевое слово <c>true</c> — логический литерал «истина».
    /// </summary>
    True,

    /// <summary>
    ///  Ключевое слово <c>false</c> — логический литерал «ложь».
    /// </summary>
    False,

    /// <summary>
    ///  Ключевое слово <c>if</c> — начало условного блока.
    /// </summary>
    If,

    /// <summary>
    ///  Ключевое слово <c>else</c> — начало альтернативной ветви условного блока.
    /// </summary>
    Else,

    /// <summary>
    ///  Ключевое слово <c>int</c> — целочисленный тип.
    /// </summary>
    Int,

    /// <summary>
    ///  Ключевое слово <c>fn</c> — объявление функции.
    /// </summary>
    Fn,

    /// <summary>
    ///  Ключевое слово <c>let</c> — объявление переменной.
    /// </summary>
    Let,

    /// <summary>
    ///  Ключевое слово <c>return</c> — возврат значения из функции.
    /// </summary>
    Return,

    /// <summary>
    ///  Ключевое слово <c>str</c> — строковый тип.
    /// </summary>
    Str,

    /// <summary>
    ///  Ключевое слово <c>struct</c> — объявление структуры.
    /// </summary>
    Struct,

    /// <summary>
    ///  Ключевое слово <c>while</c>.
    /// </summary>
    While,

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
    ///  Оператор <c>></c>.
    /// </summary>
    Less,

    /// <summary>
    ///  Оператор <c>>=</c>.
    /// </summary>
    LessOrEqual,

    /// <summary>
    ///  Оператор <c></c>.
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
}