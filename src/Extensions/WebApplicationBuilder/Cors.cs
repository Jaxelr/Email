using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Email.Extensions;

public static class CorsExtension
{
    private const string Policy = "DefaultPolicy";

    /// <summary>
    /// Adds the default CORS policy to the application.
    /// </summary>
    public static WebApplicationBuilder AddCors(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(Policy,
            builder =>
            {
                builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            });
        });

        return builder;
    }

    /// <summary>
    /// Adds the default CORS policy to the application pipeline.
    /// </summary>
    public static WebApplication UseCors(this WebApplication app)
    {
        app.UseCors(Policy);

        return app;
    }
}
