using System.Threading.Tasks;
using Carter;
using Carter.OpenApi;
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
using Newtonsoft.Json.Linq;
using Serilog;

const string ServiceName = "Email Service";
const string Policy = "DefaultPolicy";

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, services, config) =>
    config
    .ReadFrom.Configuration(ctx.Configuration)
    .ReadFrom.Services(services));

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

//Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(settings.RouteDefinition.Version, new OpenApiInfo
    {
        Description = ServiceName,
        Version = settings.RouteDefinition.Version
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

builder.Services.AddSingleton(settings); //typeof(AppSettings)
builder.Services.AddSingleton<IEmailRepository, SmtpRepository>(); //Switchable with the Sendgrid Repository

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

app.UseHealthChecks("/healthcheck", new HealthCheckOptions()
{
    ResponseWriter = WriteResponse
});

app.UseSwagger();
app.UseSwaggerUI();

app.UseEndpoints(builder => builder.MapCarter());

app.Run();

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
