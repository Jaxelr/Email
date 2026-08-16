using System;
using Email.Models;
using Email.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Polly;

namespace Email.Extensions;

public static class ServiceExtensions
{
    /// <summary>
    /// Adds the application settings, retry policy, and email repository dependencies.
    /// </summary>
    public static WebApplicationBuilder AddDependencies(this WebApplicationBuilder builder, AppSettings settings)
    {
        builder.Services.AddSingleton(_ =>
            Policy.Handle<Exception>().WaitAndRetry(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));

        builder.Services.AddSingleton(settings); //typeof(AppSettings)
        builder.Services.AddSingleton<IEmailRepository, SmtpRepository>(); //Switchable with the Sendgrid Repository

        return builder;
    }
}
