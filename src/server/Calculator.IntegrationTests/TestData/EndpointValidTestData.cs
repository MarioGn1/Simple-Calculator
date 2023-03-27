using System.Collections;

namespace Calculator.IntegrationTests.TestData;

internal class EndpointValidTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { "-(-5+2*1)", 3 };
        yield return new object[] { "-(-5+2*1) - (-5+2*1)", 6 };
        yield return new object[] { "-((-5+2*1) - (-5+2*1))", 0 };
        yield return new object[] { "-(-5+2*1) + (-5+2*1)", 0 };
        yield return new object[] { "- (1 + 2) * 4 / 6", -2 };
        yield return new object[] { "- 3 * (4 + 5) ", -27 };
        yield return new object[] { "- 2 + 3 +(4 + 5) * 6", 55 };
        yield return new object[] { "- ((1 + 2) * 4) / 6", -2 };
        yield return new object[] { "- ((1 + 2) * -4) / -6", -2 };
        yield return new object[] { "- ((1 + 2) * 4) / -6", 2 };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
