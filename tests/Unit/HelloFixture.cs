using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using EmailService;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace Api.Test.Unit
{
    public class HelloModuleFixture
    {
        private readonly HttpClient client;

        public HelloModuleFixture()
        {
            var featureCollection = new FeatureCollection();
            featureCollection.Set<IServerAddressesFeature>(new ServerAddressesFeature());

            var server = new TestServer(WebHost.CreateDefaultBuilder()
                    .UseStartup<Startup>(), featureCollection
            );

            client = server.CreateClient();
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
