using System.Threading.Tasks;
using EmailService.Models;
using Carter;

namespace EmailService.Modules
{
    public class MainModule : CarterModule
    {
        public MainModule(AppSettings app)
        {
            Get("/", (_, res) =>
            {
                res.Redirect(app.RouteDefinition.RoutePrefix);

                return Task.CompletedTask;
            });
        }
    }
}
