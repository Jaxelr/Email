FROM mcr.microsoft.com/dotnet/aspnet:8.0
LABEL name="EmailService"
COPY src/Email/bin/Release/net8.0/publish/ App/
WORKDIR /App
EXPOSE 80
ENTRYPOINT ["dotnet", "EmailService.dll"]
