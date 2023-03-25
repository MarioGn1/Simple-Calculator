using Calculator.Endpoints.Calculate;
using FastEndpoints;
using FastEndpoints.Swagger;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace Calculator.Configuration;

public static class ServiceCollectionExtensions
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

        return services;
    }
}
