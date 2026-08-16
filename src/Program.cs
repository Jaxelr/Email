using Email.Extensions;
using Email.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, services, config) =>
    config
    .ReadFrom.Configuration(ctx.Configuration)
    .ReadFrom.Services(services));

var settings = new AppSettings();

builder.Configuration.GetSection(nameof(AppSettings)).Bind(settings);

builder.AddCors();
builder.AddOpenApi(settings);

builder.AddCarter();
builder.AddDependencies(settings);
builder.AddHealthChecks();

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

app.UseHealthChecks();

app.UseOpenApi(settings);

app.UseCarter();

await app.RunAsync();
