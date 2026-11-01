namespace Lexing;

/// <summary>
///  Значение лексемы: имя идентификатора, текст строки или число.
/// </summary>
public sealed record TokenValue
{
    private readonly object value;

    public TokenValue(string value)
    {
        this.value = value;
    }

    public TokenValue(int value)
    {
        this.value = value;
    }

    /// <summary>
    ///  Возвращает значение как целое число.
    /// </summary>
    /// <exception cref="InvalidOperationException">Если значение не является числом.</exception>
    public int ToInt()
    {
        return value switch
        {
            int i => i,
            _ => throw new InvalidOperationException($"Значение \"{value}\" не является целым числом"),
        };
    }

    /// <summary>
    ///  Возвращает значение как строку.
    /// </summary>
    public override string ToString()
    {
        return value switch
        {
            string s => s,
            int i => i.ToString(System.Globalization.CultureInfo.InvariantCulture),
            _ => throw new InvalidOperationException($"Неизвестный тип значения {value.GetType()}"),
        };
    }
}