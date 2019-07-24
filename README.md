# Email Service

This service allows the user to use an smtp (or sendgrid) configuration and send emails by http requests. 

## Configurations

Some configurations that are included on the appsettings are:

1. SmtpServer - Required: the smtp server to be used for emailing.
1. Addresses - Optional: if left empty the current host is selected. Urls defined here, will be used as endpoints on the open ui page for validation.

The current appsettings.json can be configured manually:

```json
{
  "AppSettings": {
    "SmtpServer": "<SmtpServer>",
    "Addresses": [
      ""
    ]
  }
}

```

## HealthChecker

The endpoint of root/healthcheck for each requests includes a json heartbeat to determine if the service is online. This was done using the library of [Microsoft.AspnetCore.HealthChecks](https://github.com/dotnet-architecture/HealthChecks) for more information check the github repo.

## OpenApi

The OpenApi version used is Version 3.0.0