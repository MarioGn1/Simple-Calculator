using FluentAssertions;
using Calculator.Services.Contracts;
using Calculator.UniTests.TestData;
using Calculator.Services;

namespace Calculator.UniTests;

public class CalculatorServiceTests
{
    private readonly ICalculatorService _calculatorService;

    public CalculatorServiceTests(ICalculatorService calculatorService)
    {
        _calculatorService = calculatorService;
    }

    [Theory]
    [InlineData("-(-5+2*1)", 3)]
    [InlineData("-(-5+2*1) - (-5+2*1)", 6)]
    [InlineData("-(-5+2*1) + (-5+2*1)", 0)]
    [InlineData("- (1 + 2) * 4 / 6", -2)]
    [InlineData("- 3 * (4 + 5) ", -27)]
    [InlineData("- 2 + 3 +(4 + 5) * 6", 55)]
    public async Task ShouldCalculate(string expression, double expectedResult)
    {
        var result = await _calculatorService.Calculate(expression);

        result.Data.Should().Be(expectedResult);
    }

    [Theory, ClassData(typeof(CalculatorInvalidTestData))]
    public async Task Should_Fail_Due_To_Invalid_Paterns(string expression)
    {
        var result = await _calculatorService.Calculate(expression);

        result.Success.Should().BeFalse();
    }

}