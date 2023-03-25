using Calculator.Endpoints.Calculate;
using Calculator.Services;
using Calculator.Services.Contracts;
using FastEndpoints;
using FastEndpoints.Swagger;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace Calculator.Configuration;

public static class ServicesConfiguration
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        services
            .AddHealthChecks();

        services
            .AddFastEndpoints()
            .AddSwaggerDoc(settings =>
            {
                settings.Title = ServerConsts.FullName;
                settings.Version = "v1";
            });

        services
            .AddFluentValidationAutoValidation()
            .AddValidatorsFromAssemblyContaining<CalculateRequestValidator>();

        services
            .AddTransient<ICalculatorService, CalculatorService>();

        return services;
    }
}
