using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;
using models = Email.Models;

namespace EmailService.Tests.Unit;

public class EmailModuleFixture : IDisposable
{
    private readonly HttpClient client;
    private const string ApplicationJson = "application/json";

    public EmailModuleFixture()
    {
        var server = new WebApplicationFactory<Program>();

        client = server.CreateClient();
    }

    public void Dispose()
    {
        client?.Dispose();
        GC.SuppressFinalize(this);
    }

    [Fact]
    public async Task Email_module_post_email_validation_failed()
    {
        //Arrange
        var email = new models.Email();

        //Act
        var res = await client.PostAsync("/Email", new StringContent(JsonConvert.SerializeObject(email), Encoding.UTF8, ApplicationJson));

        //Assert
        Assert.Equal(HttpStatusCode.UnprocessableEntity, res.StatusCode);
    }

    [Fact]
    public async Task Email_module_post_email_validation_Ok()
    {
        //Arrange
        var email = new models.Email() { From = "notreply@mail.com", To = new string[] { "notreply@mail.com" } };

        //Act
        var res = await client.PostAsync("/Email", new StringContent(JsonConvert.SerializeObject(email), Encoding.UTF8, ApplicationJson));

        //Assert
        Assert.Equal(HttpStatusCode.OK, res.StatusCode);
    }
}
