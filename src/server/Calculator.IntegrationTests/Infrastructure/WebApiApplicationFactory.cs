using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Calculator.IntegrationTests.Infrastructure
{
    public class WebApiApplicationFactory<TEntryPoint> : WebApplicationFactory<TEntryPoint>
         where TEntryPoint : class
    {
        private static readonly Dictionary<Type, object> _applicationFactories = new();

        private readonly Action<IServiceCollection> _registerServices;

        internal static WebApiApplicationFactory<TEntryPoint> Create(Action<IServiceCollection> registerServicesAction,
            Action<WebApiApplicationFactory<TEntryPoint>> onCreationAction)
        {
            var applicationFactoryType = typeof(WebApiApplicationFactory<TEntryPoint>);

            if (_applicationFactories.ContainsKey(applicationFactoryType))
                return (WebApiApplicationFactory<TEntryPoint>)_applicationFactories[applicationFactoryType];

            var applicationFactory = new WebApiApplicationFactory<TEntryPoint>(registerServicesAction);

            _applicationFactories.Add(applicationFactoryType, applicationFactory);

            var _ = applicationFactory.Server;

            onCreationAction(applicationFactory);

            return applicationFactory;
        }

        private WebApiApplicationFactory(Action<IServiceCollection> registerServices)
        {
            _registerServices = registerServices;
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureServices((_, services) =>
            {
                _registerServices(services);
            });

            return base.CreateHost(builder);
        }
    }
}


