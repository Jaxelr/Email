using Carter;
using Microsoft.AspNetCore.Builder;

namespace Email.Extensions;

public static class CarterExtensions
{
    public static WebApplicationBuilder AddCarter(this WebApplicationBuilder builder)
    {
        builder.Services.AddCarter();

        return builder;
    }

    public static WebApplication UseCarter(this WebApplication app)
    {
        app.MapCarter();

        return app;
    }
}
