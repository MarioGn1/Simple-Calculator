using Calculator.Services.Contracts;
using FastEndpoints;

namespace Calculator.Endpoints.Calculate;

public class CalculateEndpoint : Endpoint<CalculateRequest, double>
{
    internal const string RouteUrl = "api/calculator/calculate";

    private readonly ICalculatorService _calculatorService;

    public CalculateEndpoint(ICalculatorService calculatorService)
        => _calculatorService = calculatorService;

    public override void Configure()
    {
        Post(RouteUrl);
        AllowAnonymous();
    }

    public override async Task HandleAsync(CalculateRequest req, CancellationToken cancellationToken)
    {
        var result = await _calculatorService.Calculate(req.Expression);

        if (!result.Success)
            ThrowError(string.Join(Environment.NewLine, result.ErrorMessages));

        await SendOkAsync(result.Data, cancellationToken);
    }
}
