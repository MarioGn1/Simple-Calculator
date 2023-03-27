using Calculator.Extensions;
using Calculator.Services.Contracts;
using Calculator.Services.Models;
using System.Text.RegularExpressions;

namespace Calculator.Services;

public class CalculatorService : ICalculatorService
{
    private const string _invalidExpressionTemplate = "Invalid pure math expression match: {0}";
    private const string _failToParseTemplate = "Failed to parse value: {0}";
    private const string _firstPriorityOps = "*/";
    private const string _secondPriorityOps = "+-";

    private static readonly Regex whiteSpaces = new(@"\s+");
    private static readonly Regex parenthesesExpression = new(@"[(]{1}[+\-]?\d+([\.,]{1}\d+)?([+\-]{1}|[*\/]{1}[\-]?)\d+([\.,]{1}\d+)?(([+\-]{1}|[*\/]{1}[\-]?)\d+([\.,]{1}\d+)?)*[)]{1}");
    private static readonly Regex pureMathExpression = new(@"^[+\-]?\d+([\.,]{1}\d+)?([*\/+\-]{1}[+\-]?\d+([\.,]{1}\d+)?)*$");
    private static readonly Regex multiplyDevide = new(@"[\-]?\d+([\.,]{1}\d+)?([*\/]{1}[\-]?\d+([\.,]{1}\d+)?){1}");
    private static readonly Regex addSubtractDevide = new(@"[\-]?\d+([\.,]{1}\d+)?([+\-]{1}\d+([\.,]{1}\d+)?){1}");

    public async Task<ServiceResult<double>> Calculate(string expression)
    {
        var calcResult = new ServiceResult<double>();

        expression = whiteSpaces.Replace(expression, "");

        await ExecuteCalculation(expression, calcResult);

        return calcResult;
    }

    private async Task ExecuteCalculation(string expression, ServiceResult<double> result)
    {
        var parenthesesMatches = parenthesesExpression.Matches(expression);

        if (parenthesesMatches.Count > 0)
        {
            foreach (var parenthesesMatch in parenthesesMatches)
            {
                var curSubExpression = parenthesesMatch.ToString();
                var nextSubExpression = await result.PrepareNext(curSubExpression);

                await ExecuteCalculation(nextSubExpression, result);

                if (!result.Success) return;

                expression = await result.Apply(expression, curSubExpression!);

                if (parenthesesExpression.Matches(expression).Count > 0)
                    await ExecuteCalculation(expression, result);
            }
        }

        if (parenthesesExpression.Matches(expression).Count > 0) return;

        var mathematicalMatches = pureMathExpression.Matches(expression);

        if (mathematicalMatches.Count <= 0)
            await result.AddMessage(string.Format(_invalidExpressionTemplate, expression));

        if (!result.Success) return;

        await CalculatePureExpression(expression, result);
    }

    private static async Task CalculatePureExpression(string expression, ServiceResult<double> result)
    {
        if (expression.Split((_firstPriorityOps + _secondPriorityOps).ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Length == 1)
        {
            await result.SetResult(double.Parse(expression));
            return;
        }

        var firstPriorityMatches = multiplyDevide.Matches(expression);

        await WithPriority(expression, firstPriorityMatches, _firstPriorityOps, result);

        if (firstPriorityMatches.Count <= 0)
        {
            var secondPriorityMatches = addSubtractDevide.Matches(expression);

            await WithPriority(expression, secondPriorityMatches, _secondPriorityOps, result);
        }
    }

    private static async Task WithPriority(string expression, MatchCollection matches, string priorityOperators, ServiceResult<double> result)
    {
        if (matches.Count > 0)
        {
            var firstMatchOperation = matches[0].ToString();

            await SimpleCalculation(firstMatchOperation, priorityOperators, result);

            expression = expression.Replace(firstMatchOperation, result.Data.ToString());

            expression = await result.Apply(expression, result.Data.ToString());

            await CalculatePureExpression(expression, result);
        }
    }

    private static async Task SimpleCalculation(string expression, string supportedOps, ServiceResult<double> result)
    {
        foreach (var mathOperator in supportedOps)
        {
            var simpleExpression = expression.Split(mathOperator, StringSplitOptions.RemoveEmptyEntries);
            if (simpleExpression.Length > 1)
            {
                if (!double.TryParse(simpleExpression[0].Replace(',', '.'), out var left))
                {
                    await result.AddMessage(string.Format(_failToParseTemplate, simpleExpression[0]));
                    return;
                }

                if (!double.TryParse(simpleExpression[1].Replace(',', '.'), out var right))
                {
                    await result.AddMessage(string.Format(_failToParseTemplate, simpleExpression[0]));
                    return;
                }

                await result.SetResult(mathOperator.Run(left, right));
            }
        }
    }
}
