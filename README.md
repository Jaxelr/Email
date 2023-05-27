# Email Api 

![.NET](https://github.com/Jaxelr/Email/workflows/.NET/badge.svg)

This service allows the user to use a local smtp server (or using sendgrid service) and send emails by http requests. It is build on top of aspnet and the Carter library for routing management. It includes a Swagger UI page for discoverability and it is configured to log execution, so that it is easily switchable for any context needed. 

## Configurations

Some configurations that are included on the appsettings are:

1. SmtpServer - Required if using SmtpRepository: the smtp server to be used for emailing
1. ApiKey - Required if using SendGridRepository: the Api key needed to invoke the sendgrid library
1. Addresses - Optional: if left empty the current host is selected. Urls defined here, will be used as endpoints on the open ui page for validation. If the service is running behind IIS this is needed since kestrel proxies the requests over to IIS
1. Route Definition - Required: These values are attached to the openapi declaration and are needed for the defined metadata info
   1. Route Suffix - The path where the swagger json will be shown.
   1. Version -  The swagger.json file version.

The current appsettings.json can be configured manually:

```json
{
  "Serilog": {
    "Using": [ "Serilog.Sinks.Console" ],
    "MinimumLevel": "Information",
    "WriteTo": [
      { "Name": "Console" }
    ],
    "Enrich": [ "FromLogContext", "WithMachineName", "WithThreadId" ],
    "AllowedHosts": "*"
  },
  "AppSettings": {
    "SmtpServer": "<SmtpServer>",
    "ApiKey": "<KeyGoesHere>",
    "Addresses": [
      ""
    ],
    "RouteDefinition": {
      "RouteSuffix": "swagger",
      "Version": "v1"
    }
  }
}

```

## HealthChecker

The endpoint of root/healthcheck for each requests includes a json heartbeat to determine if the service is online. This was done using the library of [Microsoft.Extensions.Diagnostics.HealthChecks](https://github.com/dotnet/aspnetcore/tree/master/src/HealthChecks) for more information check the github repo.

## OpenApi

The OpenApi version used is Version 3.0.1

## Dependencies & Libraries

This project depends on net 7.0. The following oss libraries are used on this repo as dependencies:

- [Carter](https://github.com/CarterCommunity/Carter)
- [Xunit](https://github.com/xunit/xunit)
- [Swashbuckle.AspNetCore](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)
- [Serilog.AspnetCore](https://github.com/serilog/serilog-aspnetcore/)
- [Sendgrid](https://github.com/sendgrid/sendgrid-csharp)
- [NSubstitute](https://github.com/nsubstitute/NSubstitute)
