using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace EmailService.Tests.Unit;

public class OpenApiTests
{
    [Fact]
    public async Task OpenApi_document_preserves_version_3_1()
    {
        using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient();

        var document = await client.GetFromJsonAsync<JsonElement>("/openapi/v1.json", TestContext.Current.CancellationToken);

        Assert.StartsWith("3.1.", document.GetProperty("openapi").GetString());
    }
}
