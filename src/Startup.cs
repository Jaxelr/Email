using System.Collections.Generic;
using System.Threading.Tasks;
using Carter;
using EmailService.Entities;
using EmailService.Extensions;
using EmailService.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.HealthChecks;
using Microsoft.Extensions.Hosting;

namespace EmailService
{
    public class Startup
    {
        private IConfiguration Configuration { get; set; }

        private readonly AppSettings settings = new AppSettings();

        private const string ServiceName = "Email Service";

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
              .SetBasePath(env.ContentRootPath)
              .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
              .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
              .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //HealthChecks
            services.AddHealthChecks(checks =>
            {
                checks.AddValueTaskCheck("HTTP Endpoint", () => new
                    ValueTask<IHealthCheckResult>(HealthCheckResult.Healthy("Ok")));
            });

            //Extract the AppSettings information from the appsettings config.
            Configuration.GetSection(nameof(AppSettings)).Bind(settings);

            services.AddSingleton(settings); //AppSettings type

            services.AddTransient<IEmailRepository>(imp => new SmtpRepository(settings.SmtpServer));

            services.AddCarter();
            services.AddMemoryCache();
        }

        public void Configure(IApplicationBuilder app, AppSettings appSettings)
        {
            ICollection<string> addresses = appSettings.Addresses.Empty() ?
                                app.ServerFeatures.Get<IServerAddressesFeature>().Addresses :
                                appSettings.Addresses;

            app.UseCarter(GetOptions(addresses));

            app.UseSwaggerUI(opt =>
            {
                opt.RoutePrefix = appSettings.RouteDefinition.RoutePrefix;
                opt.SwaggerEndpoint(appSettings.RouteDefinition.SwaggerEndpoint, ServiceName);
            });
        }

        private CarterOptions GetOptions(ICollection<string> addresses) =>
            new CarterOptions(openApiOptions: new OpenApiOptions(ServiceName, addresses, new Dictionary<string, OpenApiSecurity>()));
    }
}
