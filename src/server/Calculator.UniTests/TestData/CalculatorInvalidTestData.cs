using System.Collections;

namespace Calculator.UniTests.TestData;

public class CalculatorInvalidTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { "--(-5+2*1)" };
        yield return new object[] { "-(-5.,2+2*1)" };
        yield return new object[] { "(-5..2+2*1)" };
        yield return new object[] { "(-5,,2+2*1)" };
        yield return new object[] { "(-5.+2*1)" };
        yield return new object[] { "(-5,+2*1)" };
        yield return new object[] { "(-5++2*1)" };
        yield return new object[] { "5+2a)" };
        yield return new object[] { "5+2.2.2)" };
        yield return new object[] { "5+2,2,2)" };
        yield return new object[] { "5+2.2,2)" };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
