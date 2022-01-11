using System.Collections.Generic;

namespace Email.Models;

public record AppSettings
{
    public string SmtpServer { get; init; }
    public string ApiKey { get; init; }
    public RouteDefinition RouteDefinition { get; init; }
    public ICollection<string> ServerUrls { get; init; }
}
