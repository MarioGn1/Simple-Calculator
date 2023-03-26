using Calculator.Services.Models;

namespace Calculator.Extensions;

public static class ServiceResultExtensions
{
    private const string _invalidParenthesesTemplate = "Invalid expression match: {0}";

    public static Task<string> Apply(this ServiceResult<double> result, string expression, string subExpression)
    {
        var previousMathOperatorIndex = expression.IndexOf(subExpression) - 1;

        if (previousMathOperatorIndex < 0) return Task.FromResult(expression);

        var previousMathOperator = expression[previousMathOperatorIndex];
        var resultSign = result.Data < 0 ? '-' : '+';
        var resultData = Math.Abs(result.Data);

        string resultExpression;

        if (previousMathOperator.Equals('-') && resultSign.Equals('-'))
            resultExpression = expression.Replace(previousMathOperator + subExpression, "+" + resultData);
        else if (previousMathOperator.Equals('-') || resultSign.Equals('-'))
            resultExpression = expression.Replace(previousMathOperator + subExpression, "-" + resultData);
        else
            resultExpression = expression.Replace(subExpression, resultData.ToString());

        return Task.FromResult(resultExpression);
    }

    public static Task<string> PrepareNext(this ServiceResult<double> result, string? curMatch)
    {
        if (string.IsNullOrEmpty(curMatch))
            result.AddMessage(string.Format(_invalidParenthesesTemplate, curMatch));

        return Task.FromResult(curMatch!.RemoveOutherParentheses());
    }
}
