using FastEndpoints;

namespace Calculator.Endpoints.Calculate;

public class CalculateEndpoint : Endpoint<CalculateRequest, double>
{
    internal const string RouteUrl = "api/calculator/calculate";

    public override void Configure()
    {
        Post(RouteUrl);
        AllowAnonymous();
    }

    public override async Task HandleAsync(CalculateRequest req, CancellationToken cancellationToken)
    {
        var response = 1.1;

        await SendOkAsync(response, cancellationToken);
    }
}
