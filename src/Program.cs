using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace EmailService
{
    public class Program
    {
        public static void Main()
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
