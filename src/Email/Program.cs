using System.Collections.Generic;
using System.Threading.Tasks;
using Carter;
using Email.Models;
using Email.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

const string ServiceName = "Email Service";
const string Policy = "DefaultPolicy";

var builder = WebApplication.CreateBuilder(args);

var settings = new AppSettings();

builder.Configuration.GetSection(nameof(AppSettings)).Bind(settings);

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

builder.Services.AddLogging(opt =>
{
    opt.ClearProviders();
    opt.AddConsole();
    opt.AddDebug();
    opt.AddConfiguration(builder.Configuration.GetSection("Logging"));
});

builder.Services.AddCarter(options => options.OpenApi = GetOpenApiOptions(settings));

builder.Services.AddSingleton(settings); //typeof(AppSettings)
builder.Services.AddSingleton<IEmailRepository, SmtpRepository>();

//HealthChecks
builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseCors(Policy);

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseSwaggerUI(opt =>
{
    opt.RoutePrefix = settings.RouteDefinition.RoutePrefix;
    opt.SwaggerEndpoint(settings.RouteDefinition.SwaggerEndpoint, ServiceName);
});

app.UseHealthChecks("/healthcheck", new HealthCheckOptions()
{
    ResponseWriter = WriteResponse
});

app.UseEndpoints(builder => builder.MapCarter());

app.Run();

static OpenApiOptions GetOpenApiOptions(AppSettings settings) =>
        new()
        {
            DocumentTitle = ServiceName,
            ServerUrls = settings.ServerUrls,
            Securities = new Dictionary<string, OpenApiSecurity>()
        };

static Task WriteResponse(HttpContext context, HealthReport report)
{
    context.Response.ContentType = "application/json";

    var json = new JObject(
                new JProperty("statusCode", report.Status),
                new JProperty("status", report.Status.ToString()),
                new JProperty("timelapsed", report.TotalDuration)
        );

    return context.Response.WriteAsync(json.ToString(Newtonsoft.Json.Formatting.Indented));
}
