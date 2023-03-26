namespace Calculator.Extensions;

public static class StringExtensions
{
    public static string RemoveOutherParentheses(this string expression)
    {
        var startParenthesis = expression.IndexOf('(');
        var endParenthesis = expression.LastIndexOf(')');

        return expression.Remove(startParenthesis, 1).Remove(endParenthesis - 1);
    }
}
