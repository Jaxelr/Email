using Api.Model.Properties;
using Funq;
using NotDeadYet;
using Repoes;
using ServiceStack.Api.Swagger;
using ServiceStack.ServiceInterface.Cors;
using ServiceStack.ServiceInterface.Validation;
using ServiceStack.ServiceModel.Serialization;
using ServiceStack.Text;
using ServiceStack.WebHost.Endpoints;
using System.Xml;

public class AppHost : AppHostBase
{
    /// <summary>
    ///     Initializes a new instance of your ServiceStack application, with the specified name and assembly containing the services.
    /// </summary>
    public AppHost() : base(Default.ServiceName, typeof(AppHost).Assembly) { }

    public override void Configure(Container container)
    {
        JsConfig.DateHandler = JsonDateHandler.ISO8601;

        //Set JSON web services to return idiomatic JSON camelCase properties
        JsConfig.EmitCamelCaseNames = true;

        Plugins.Add(new CorsFeature()); //Enable CORS
        Plugins.Add(new ValidationFeature());
        Plugins.Add(new SwaggerFeature());

        SetConfig(new EndpointHostConfig
        {
#if DEBUG
            DebugMode = true,
#endif
            Return204NoContentForEmptyResponse = true,
            //These are used for Soap compatibility.
            SoapServiceName = Default.ServiceName,
            WsdlServiceNamespace = Default.Namespace,
            WsdlSoapActionNamespace = Default.Namespace
        });

        //Health Checker
        var thisAssembly = typeof(AppHost).Assembly;
        var notDeadYetAssembly = typeof(IHealthChecker).Assembly;

        var healthChecker = new HealthCheckerBuilder()
            .WithHealthChecksFromAssemblies(thisAssembly, notDeadYetAssembly)
            .Build();

        container.Register(healthChecker);

        /*
            The attachments could be huge, so we must provide a way for the Soap client to not get a
            rejection for Max array quota. We override the singleton serializer of Soap.
        */
        DataContractDeserializer.Instance = new DataContractDeserializer(new XmlDictionaryReaderQuotas
        {
            MaxArrayLength = 2147483646
        });

        container.Register<IEmailRepository>(e => new SmtpRepository(Default.GetAppSettingValue(Default.AppSettingsKeys.EmailHost)));
    }
}
