namespace Execution;

/// <summary>
///  Окружение программы: ввод, вывод и вывод ошибок.
/// </summary>
public interface IEnvironment
{
    /// <summary>
    ///  Читает строку из ввода без символа переноса строки.
    ///  Возвращает <c>null</c>, если ввод закончился.
    /// </summary>
    string? ReadLine();

    /// <summary>
    ///  Записывает текст в вывод.
    /// </summary>
    void Write(string text);

    /// <summary>
    ///  Записывает сообщение об ошибке в поток ошибок.
    /// </summary>
    void WriteError(string message);
}