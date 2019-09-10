using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace EmailService
{
    public class Program
    {
#pragma warning disable IDE0060 // Remove unused parameter

        public static void Main(string[] args)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            var host = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseKestrel()
                .UseIISIntegration()
                .UseHealthChecks("/healthcheck")
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
