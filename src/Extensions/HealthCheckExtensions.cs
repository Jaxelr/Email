using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Email.Extensions;

public static class HealthCheckExtensions
{
    /// <summary>
    /// Adds health-check services to the application.
    /// </summary>
    public static WebApplicationBuilder AddHealthChecks(this WebApplicationBuilder builder)
    {
        builder.Services.AddHealthChecks();

        return builder;
    }

    /// <summary>
    /// Adds the health-check endpoint to the application pipeline.
    /// </summary>
    public static WebApplication UseHealthChecks(this WebApplication app)
    {
        app.UseHealthChecks("/healthcheck", new HealthCheckOptions
        {
            ResponseWriter = WriteResponse
        });

        return app;
    }

    /// <summary>
    /// Writes the health-check report as JSON.
    /// </summary>
    private static Task WriteResponse(HttpContext context, HealthReport report)
    {
        context.Response.ContentType = "application/json";

        var json = new
        {
            statusCode = report.Status,
            status = report.Status.ToString(),
            timelapsed = report.TotalDuration
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(json));
    }
}
