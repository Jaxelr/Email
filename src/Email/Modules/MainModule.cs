using System.Threading.Tasks;
using Carter;
using Email.Models;

namespace EmailService.Modules;

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
