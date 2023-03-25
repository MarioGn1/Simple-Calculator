using FastEndpoints;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Calculator.Endpoints.Calculate;

public class CalculateRequestValidator : Validator<CalculateRequest>
{
    private const int _maxLength = 200;

    private const string invalidExpressionTemplate = "Invalid expression: missing or wrong parentheses patern";
    private const string invalidMaxLengthTemplate = "The expressions is too long: current limit is {0} characters";
    private const string invalidSimbolsTemplate = "Invalid expression simbol(s) found";

    private const string validSimbolsPatern = @"^[\d,+,\-,*,\/,(,),\s]*$";
    private readonly Regex validSimbols = new(validSimbolsPatern);

    public CalculateRequestValidator()
    {
        RuleFor(x => x.Expression)
            .NotEmpty()
            .MustAsync(ValidateExpression)
            .WithMessage(invalidExpressionTemplate)
            .Matches(validSimbols)
            .WithMessage(invalidSimbolsTemplate);

        RuleFor(x => x.Expression.Length)
            .LessThan(_maxLength)
            .WithMessage(string.Format(invalidMaxLengthTemplate, _maxLength));
    }

    private Task<bool> ValidateExpression(CalculateRequest _, string expression, CancellationToken cancellationToken)
    {
        var parentheses = expression.Where(x => x.Equals('(') || x.Equals(')')).ToList();

        if (!parentheses.Any()) return Task.FromResult(true);

        var parenthesesIndicator = 0;

        foreach (var parenthesis in parentheses)
        {
            if (parenthesis.Equals('('))
            {
                parenthesesIndicator++;
                continue;
            }

            parenthesesIndicator--;

            if (parenthesesIndicator < 0)
                return Task.FromResult(false);
        }

        if (parenthesesIndicator != 0)
            return Task.FromResult(false);

        return Task.FromResult(true);
    }
}
