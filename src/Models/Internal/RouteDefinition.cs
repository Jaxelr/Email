namespace Email.Models;

public record RouteDefinition
{
    public string Resource { get; init; } = string.Empty;
    public string Version { get; init; } = string.Empty;
}
