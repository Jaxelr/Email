using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;
using Models = Email.Models;

namespace EmailService.Tests.Unit;

public class EmailModuleTests : IDisposable
{
    private readonly HttpClient client;
    private const string ApplicationJson = "application/json";

    public EmailModuleTests()
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
        var email = new Models.Email();

        //Act
        var res = await client.PostAsync("/Email", new StringContent(JsonConvert.SerializeObject(email), Encoding.UTF8, ApplicationJson));

        //Assert
        Assert.Equal(HttpStatusCode.UnprocessableEntity, res.StatusCode);
    }

    [Fact]
    public async Task Email_module_post_email_validation_missing_body_failed()
    {
        //Arrange
        var email = new Models.Email() { From = "notreply@mail.com", To = new string[] { "notreply@mail.com" } };

        //Act
        var res = await client.PostAsync("/Email", new StringContent(JsonConvert.SerializeObject(email), Encoding.UTF8, ApplicationJson));

        //Assert
        Assert.Equal(HttpStatusCode.UnprocessableEntity, res.StatusCode);
    }

    [Fact]
    public async Task Email_module_post_email_validation_ok()
    {
        //Arrange
        var email = new Models.Email() { From = "notreply@mail.com", To = new string[] { "notreply@mail.com" }, Body = "Test", Subject = "Ok" };

        //Act
        var res = await client.PostAsync("/Email", new StringContent(JsonConvert.SerializeObject(email), Encoding.UTF8, ApplicationJson));

        //Assert
        Assert.Equal(HttpStatusCode.OK, res.StatusCode);
    }

}
