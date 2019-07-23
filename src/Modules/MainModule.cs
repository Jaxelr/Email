using System.Threading.Tasks;
using EmailService.Entities;
using Carter;

namespace EmailService.Modules
{
    public class MainModule : CarterModule
    {
        public MainModule()
        {
            Get("/", (req, res, routeData) =>
            {
                res.Redirect(RouteDefinition.RoutePrefix);

                return Task.CompletedTask;
            });
        }
    }
}
