using System;
using System.Net.Http;
using EmailService;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.TestHost;

namespace Api.Test.Unit
{
    public class HelloModuleFixture : IDisposable
    {
        private readonly HttpClient client;
        private readonly TestServer server;

        public HelloModuleFixture()
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
        }

        //[Fact]
        //public async Task Hello_module_get_hello_world()
        //{
        //    //Arrange
        //    string name = "myUser";

        //    //Act
        //    var res = await client.GetAsync($"/hello/{name}");

        //    //Assert
        //    Assert.Equal(HttpStatusCode.OK, res.StatusCode);
        //    Assert.Contains(name, await res.Content.ReadAsStringAsync());
        //}
    }
}
