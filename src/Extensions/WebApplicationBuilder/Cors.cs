using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Email.Extensions;

public static class CorsExtension
{
    private const string Policy = "DefaultPolicy";

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

    public static WebApplication UseCors(this WebApplication app)
    {
        app.UseCors(Policy);

        return app;
    }
}
