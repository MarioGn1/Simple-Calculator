using FastEndpoints.Swagger;
using FastEndpoints;

namespace Calculator.Configuration;

public static class AppConfiguration
{
    public static WebApplication ConfigureApp(this WebApplication app)
    {
        app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

        app.UseHttpsRedirection();
        app.UseRouting();

        app
            .UseFastEndpoints()
            .UseOpenApi()
            .UseSwaggerUi3(c => c.ConfigureDefaults())
            .UseEndpoints(endpoints =>
                {
                    endpoints.MapHealthChecks("/health");
                });

        return app;
    }
}
