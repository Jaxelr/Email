using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Xunit;

namespace EmailService.Tests.Unit
{
    public class EmailModuleFixture : IDisposable
    {
        private readonly HttpClient client;
        private readonly TestServer server;

        public EmailModuleFixture()
        {
            var featureCollection = new FeatureCollection();
            featureCollection.Set<IServerAddressesFeature>(new ServerAddressesFeature());

            server = new TestServer(WebHost.CreateDefaultBuilder()
                    .UseStartup<Startup>(), featureCollection
            );

            client = server.CreateClient();
        }

        public void Dispose()
        {
            client?.Dispose();
            server?.Dispose();
            GC.SuppressFinalize(this);
        }

        [Fact]
        public async Task Email_module_post_email_validation_failed()
        {
            //Arrange
            var email = new Models.Email();

            //Act
            var res = await client.PostAsync("/Email", new StringContent(JsonConvert.SerializeObject(email)));

            //Assert
            Assert.Equal(HttpStatusCode.UnprocessableEntity, res.StatusCode);
        }

        [Fact]
        public async Task Email_module_post_email_validation_exception()
        {
            //Arrange
            var email = new Models.Email() { From = "notreply@mail.com", To = new string[] { "notreply@mail.com" } };

            //Act
            var res = await client.PostAsync("/Email", new StringContent(JsonConvert.SerializeObject(email)));

            //Assert
            Assert.Equal(HttpStatusCode.OK, res.StatusCode);
        }
    }
}
