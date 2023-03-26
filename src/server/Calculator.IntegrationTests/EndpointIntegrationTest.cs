using Calculator.Endpoints.Calculate;
using Calculator.IntegrationTests.Exceptions;
using Calculator.IntegrationTests.Infrastructure;
using Calculator.IntegrationTests.TestData;
using Calculator.IntegrationTests.Utils;
using FluentAssertions;

namespace Calculator.IntegrationTests;

public class EndpointIntegrationTest : IntegrationTestBase<ServerConsts>
{
    [Theory, ClassData(typeof(EndpointValidTestData))]
    public async Task Post_request_should_return_valid_result(string expression, double expectedResult)
    {
        var client = ApplicationFactory.CreateClient();

        var response = await client.Calculate(new CalculateRequest(expression));

        response.Should().NotBeNull();
        response.Should().Be(expectedResult);
    }

    [Theory, ClassData(typeof(EndpointInvalidTestData))]
    public async Task Post_request_should_return_bad_request(string expression)
    {
        var client = ApplicationFactory.CreateClient();

        var request = async () => await client.Calculate(new CalculateRequest(expression));

        (await request.Should().ThrowAsync<HttpResponseMessageStatusCodeInvalidException>())
            .Which.ReceivedStatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
