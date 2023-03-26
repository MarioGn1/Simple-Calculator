namespace Calculator.Extensions;

public static class CharExtensions
{
    public static double Run(this char op, double left, double right)
    {
        switch (op)
        {
            case '+':
                return left + right;
            case '-':
                return left - right;
            case '/':
                return left / right;
            case '*':
                return left * right;
        }
        throw new NotSupportedException();
    }
}
