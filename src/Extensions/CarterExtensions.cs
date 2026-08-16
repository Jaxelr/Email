using Carter;
using Microsoft.AspNetCore.Builder;

namespace Email.Extensions;

public static class CarterExtensions
{
    /// <summary>
    /// Adds Carter services to the application.
    /// </summary>
    public static WebApplicationBuilder AddCarter(this WebApplicationBuilder builder)
    {
        builder.Services.AddCarter();

        return builder;
    }

    /// <summary>
    /// Maps Carter routes to the application.
    /// </summary>
    public static WebApplication UseCarter(this WebApplication app)
    {
        app.MapCarter();

        return app;
    }
}
