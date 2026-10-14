namespace Lexing;

/// <summary>
///  Сканер текста: хранит исходный текст и позицию чтения.
///  Позволяет просматривать символы вперёд и сдвигать позицию только вперёд.
/// </summary>
public class TextScanner
{
    /// <summary>
    ///  Символ, возвращаемый при чтении за пределами текста.
    /// </summary>
    public const char EndOfText = '\0';

    private readonly string text;
    private int position;

    public TextScanner(string text)
    {
        this.text = text;
    }

    /// <summary>
    ///  Номер текущей строки (начиная с 1).
    /// </summary>
    public int Line { get; private set; } = 1;

    /// <summary>
    ///  Номер текущего столбца (начиная с 1).
    /// </summary>
    public int Column { get; private set; } = 1;

    /// <summary>
    ///  Возвращает символ на расстоянии <paramref name="offset"/> от позиции чтения
    ///  либо <see cref="EndOfText"/>, если текст закончился.
    /// </summary>
    public char Peek(int offset = 0)
    {
        int index = position + offset;
        return index < text.Length ? text[index] : EndOfText;
    }

    /// <summary>
    ///  Сдвигает позицию чтения на один символ вперёд.
    /// </summary>
    public void Advance()
    {
        if (position >= text.Length)
        {
            return;
        }

        // Пара CR LF считается одним переносом строки.
        char current = text[position];
        if (current == '\n' || (current == '\r' && Peek(1) != '\n'))
        {
            Line++;
            Column = 1;
        }
        else if (current != '\r')
        {
            Column++;
        }

        position++;
    }

    /// <summary>
    ///  Проверяет, что текст закончился.
    /// </summary>
    public bool IsEnd()
    {
        return position >= text.Length;
    }
}