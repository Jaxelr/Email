using Email.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;
using Scalar.AspNetCore;

namespace Email.Extensions;

public static class OpenApiExtensions
{
    private const string ServiceName = "Email Service";

    public static WebApplicationBuilder AddOpenApi(this WebApplicationBuilder builder, AppSettings settings)
    {
        builder.Services.AddOpenApi(settings.RouteDefinition?.Version!, options =>
        {
            options.AddDocumentTransformer((document, _, _) =>
            {
                document.Info = new OpenApiInfo
                {
                    Description = ServiceName,
                    Version = settings.RouteDefinition?.Version,
                };
                return System.Threading.Tasks.Task.CompletedTask;
            });
        });

        return builder;
    }

    public static WebApplication UseOpenApi(this WebApplication app, AppSettings settings)
    {
        app.MapOpenApi($"{settings.RouteDefinition?.Resource}/{settings.RouteDefinition?.Version}.json");
        app.MapScalarApiReference($"{settings.RouteDefinition?.Resource}");
        return app;
    }
}
