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
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
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

//Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Description = ServiceName,
        Version = "v1"
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
