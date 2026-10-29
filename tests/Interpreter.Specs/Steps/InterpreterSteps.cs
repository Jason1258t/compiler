using Interpreter.Specs.Fakes;

using Reqnroll;

using Xunit;

namespace Interpreter.Specs.Steps;

/// <summary>
///  Шаги сценариев: запуск интерпретатора с заданными программой и вводом и проверка результата.
/// </summary>
[Binding]
public sealed class InterpreterSteps
{
    private string sourceCode = string.Empty;
    private string[] inputLines = [];
    private FakeEnvironment? environment;
    private bool isSuccess;

    [Given("дана программа:")]
    public void GivenProgram(string code)
    {
        sourceCode = code;
    }

    [Given("дан ввод:")]
    public void GivenInput(string input)
    {
        inputLines = NormalizeLineEndings(input).Split('\n');
    }

    [When("я запускаю программу")]
    public void WhenIRunProgram()
    {
        environment = new FakeEnvironment(inputLines);
        DotScriptInterpreter interpreter = new(environment);
        isSuccess = interpreter.Run(sourceCode);
    }

    [Then("я увижу строки:")]
    public void ThenISeeLines(string expected)
    {
        AssertSuccess();
        Assert.Equal(NormalizeLineEndings(expected) + "\n", GetEnvironment().Output);
    }

    [Then("я увижу текст {string}")]
    public void ThenISeeText(string expected)
    {
        AssertSuccess();
        Assert.Equal(expected, GetEnvironment().Output);
    }

    [Then("я не увижу вывода")]
    public void ThenISeeNoOutput()
    {
        AssertSuccess();
        Assert.Equal(string.Empty, GetEnvironment().Output);
    }

    [Then("выполнение завершится ошибкой:")]
    public void ThenExecutionFailsWithError(string expected)
    {
        Assert.False(isSuccess);
        Assert.Equal([expected], GetEnvironment().Errors);
    }

    [Then("до ошибки я увижу текст {string}")]
    public void ThenBeforeErrorISeeText(string expected)
    {
        Assert.False(isSuccess);
        Assert.Equal(expected, GetEnvironment().Output);
    }

    private static string NormalizeLineEndings(string text)
    {
        return text.Replace("\r\n", "\n", StringComparison.Ordinal);
    }

    private void AssertSuccess()
    {
        Assert.Empty(GetEnvironment().Errors);
        Assert.True(isSuccess);
    }

    private FakeEnvironment GetEnvironment()
    {
        return environment ?? throw new InvalidOperationException("Программа не была запущена");
    }
}