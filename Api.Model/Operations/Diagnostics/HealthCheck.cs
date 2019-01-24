using ServiceStack.ServiceHost;

namespace Api.Model.Operations
{
    [Api("Health Checker")]
    [Route("/healthcheck", Verbs = "GET", Summary = "Checks if the website is online", Notes = "Health Check")]
    public class HealthCheck
    {
    }
}