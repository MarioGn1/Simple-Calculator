namespace Calculator.IntegrationTests.Exceptions;

public sealed class HttpResponseMessageStatusCodeInvalidException : InvalidOperationException
{
    public HttpResponseMessageStatusCodeInvalidException(HttpStatusCode? expectedStatusCode, HttpStatusCode receivedStatusCode, string? receivedContent)
        : base($"Expected status code '{expectedStatusCode?.ToString() ?? "not provided"}' but was '{receivedStatusCode}'. Received content = '{receivedContent}'")
    {
        ExpectedStatusCode = expectedStatusCode;
        ReceivedStatusCode = receivedStatusCode;
        ReceivedContent = receivedContent;
    }

    public HttpStatusCode? ExpectedStatusCode { get; }
    public HttpStatusCode ReceivedStatusCode { get; }
    public string? ReceivedContent { get; }
}
