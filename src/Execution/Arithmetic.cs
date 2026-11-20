namespace Execution;

/// <summary>
///  Арифметические операции над 32-битными целыми числами с проверкой переполнения и деления на ноль.
/// </summary>
public static class Arithmetic
{
    public static int Add(int left, int right)
    {
        return Checked(() => checked(left + right));
    }

    public static int Subtract(int left, int right)
    {
        return Checked(() => checked(left - right));
    }

    public static int Multiply(int left, int right)
    {
        return Checked(() => checked(left * right));
    }

    /// <summary>
    ///  Делит с округлением к нулю.
    /// </summary>
    public static int Divide(int left, int right)
    {
        CheckDivisor(right);
        return Checked(() => left / right);
    }

    /// <summary>
    ///  Возвращает остаток от деления, знак которого совпадает со знаком делимого.
    /// </summary>
    public static int Modulo(int left, int right)
    {
        CheckDivisor(right);
        return Checked(() => left % right);
    }

    public static int Negate(int value)
    {
        return Checked(() => checked(-value));
    }

    private static void CheckDivisor(int divisor)
    {
        if (divisor == 0)
        {
            throw new RuntimeException("деление на ноль");
        }
    }

    /// <summary>
    ///  Выполняет операцию, заменяя переполнение на ошибку выполнения.
    /// </summary>
    private static int Checked(Func<int> operation)
    {
        try
        {
            return operation();
        }
        catch (OverflowException)
        {
            throw new RuntimeException("переполнение целого числа");
        }
    }
}