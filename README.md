# Email Service

This service allows the user to use an smtp (or sendgrid) configuration and send emails by http requests. 

## Configurations

Some configurations that are included on the appsettings are:

1. SmtpServer - Required: the smtp server to be used for emailing.
1. Addresses - Optional: if left empty the current host is selected. Urls defined here, will be used as endpoints on the open ui page for validation.
1. Route Definition - Required: These values are attached to the openapi declaration and are needed for the defined metadata info
   1. Route Prefix - The path where the ui will be shown.
   1. Swagger Endpoint - The path where the openapi json metadata will be found.

The current appsettings.json can be configured manually:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Warning"
    },
    "Console": {
      "IncludeScopes": false
    }
  },
  "AppSettings": {
    "SmtpServer": "<SmtpServer>",
    "Addresses": [
      ""
    ],
    "RouteDefinition": {
      "RoutePrefix": "openapi/ui",
      "SwaggerEndpoint": "/openapi"
    }
  }
}

```

## HealthChecker

The endpoint of root/healthcheck for each requests includes a json heartbeat to determine if the service is online. This was done using the library of [Microsoft.AspnetCore.HealthChecks](https://github.com/dotnet/aspnetcore/tree/master/src/HealthChecks) for more information check the github repo.

## OpenApi

The OpenApi version used is Version 3.0.0

## Dependencies & Libraries

This project depends on dotnetcore 3.1. The following oss libraries are used on this repo as dependencies:

- [Carter](https://github.com/CarterCommunity/Carter)
- [Xunit](https://github.com/xunit/xunit)
- [Swashbuckle.Swaggerui](https://github.com/domaindrivendev/Swashbuckle)

