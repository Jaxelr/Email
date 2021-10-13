using System.Threading.Tasks;
using Email.Models;
using Carter;

namespace EmailService.Modules
{
    public class MainModule : CarterModule
    {
        public MainModule(AppSettings app)
        {
            Get("/", (ctx) =>
            {
                ctx.Response.Redirect(app.RouteDefinition.RoutePrefix);

                return Task.CompletedTask;
            });
        }
    }
}
