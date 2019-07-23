using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Carter;
using EmailService.Entities;
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
            ICollection<string> addresses = Empty(appSettings.Addresses) ?
                                app.ServerFeatures.Get<IServerAddressesFeature>().Addresses :
                                appSettings.Addresses;

            app.UseCarter(GetOptions(addresses));

            app.UseSwaggerUI(opt =>
            {
                opt.RoutePrefix = RouteDefinition.RoutePrefix;
                opt.SwaggerEndpoint(RouteDefinition.SwaggerEndpoint, ServiceName);
            });
        }

        private CarterOptions GetOptions(ICollection<string> addresses) =>
            new CarterOptions(openApiOptions: new OpenApiOptions(ServiceName, addresses, new Dictionary<string, OpenApiSecurity>()));

        /// <summary>
        /// Since the mapping from the DI returns an instantiated class as based on the appsettings.json
        /// config file, we must check if the only element included is not empty, then we can define it as empty.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        private bool Empty(ICollection<string> collection) =>
            (collection.Count == 1 && string.IsNullOrEmpty(collection.FirstOrDefault())) ?
            true :
            false;
    }
}
