using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Calculator.IntegrationTests.Infrastructure
{
    [Collection("Sequential")]
    public abstract class IntegrationTestBase<TEntryPoint>
        where TEntryPoint : class
    {
        protected IntegrationTestBase()
        {
            ApplicationFactory = WebApiApplicationFactory<TEntryPoint>.Create(RegisterCustomServices, OnApplicationFactoryCreation);
        }

        protected WebApiApplicationFactory<TEntryPoint> ApplicationFactory { get; }

        protected virtual void RegisterCustomServices(IServiceCollection serviceCollection)
        {
            // Override to register custom services.
        }

        protected virtual void OnApplicationFactoryCreation(WebApiApplicationFactory<TEntryPoint> factory)
        {
            // Override for things that only need to run once per application factory!
        }

        protected TService GetRequiredService<TService>() where TService : notnull
            => ApplicationFactory.Services.GetRequiredService<TService>();
    }
}
