using System.Collections.Generic;

namespace Email.Models;

public record AppSettings
{
    public string SmtpServer { get; init; } = string.Empty;
    public string ApiKey { get; init; } = string.Empty;
    public RouteDefinition? RouteDefinition { get; init; }
    public ICollection<string>? ServerUrls { get; init; }
}
