using Calculator.Services;
using Calculator.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Calculator.UniTests;

internal class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<ICalculatorService, CalculatorService>();
    }
}
