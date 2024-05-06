using System;
using System.Text.Json;
using System.Threading.Tasks;
using Carter;
using Carter.OpenApi;
using Email.Extensions;
using Email.Models;
using Email.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Polly;
using Serilog;

const string ServiceName = "Email Service";


var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, services, config) =>
    config
    .ReadFrom.Configuration(ctx.Configuration)
    .ReadFrom.Services(services));

var settings = new AppSettings();

builder.Configuration.GetSection(nameof(AppSettings)).Bind(settings);

builder.AddCors();

//Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(settings.RouteDefinition?.Version, new OpenApiInfo
    {
        Description = ServiceName,
        Version = settings.RouteDefinition?.Version
    });

    options.DocInclusionPredicate((_, description) =>
    {
        foreach (object metaData in description.ActionDescriptor.EndpointMetadata)
        {
            if (metaData is IIncludeOpenApi)
            {
                return true;
            }
        }
        return false;
    });
});

builder.Services.AddCarter();

builder.Services.AddSingleton(builder =>
{
    return Policy.Handle<Exception>().WaitAndRetry(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
});

builder.Services.AddSingleton(settings); //typeof(AppSettings)
builder.Services.AddSingleton<IEmailRepository, SmtpRepository>(); //Switchable with the Sendgrid Repository

//HealthChecks
builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseCors();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseHealthChecks("/healthcheck", new HealthCheckOptions()
{
    ResponseWriter = WriteResponse
});

app.UseSwagger();
app.UseSwaggerUI();

app.MapCarter();

app.Run();

static Task WriteResponse(HttpContext context, HealthReport report)
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
