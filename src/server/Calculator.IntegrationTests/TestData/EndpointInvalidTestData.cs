using System.Collections;

namespace Calculator.IntegrationTests.TestData;

internal class EndpointInvalidTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { ""};
        yield return new object[] { "-(-5+2*1"};
        yield return new object[] { "-(-5+2*1))"};
        yield return new object[] { "-5+2*1)"};
        yield return new object[] { "(- (1 + 2) * 4) / 6)"};
        yield return new object[] { "- a * (4 + 5) "};
        yield return new object[] { "- 2 + 3 +(4 + 5) = 6"};
        yield return new object[] { "- 2 + 3 +(4_4 + 5) * 6"};
        yield return new object[] { "- 2 + 3 +(4^4 + 5) * 6"};
        yield return new object[] { "- 2 + 3 +(4%4 + 5) * 6"};
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
