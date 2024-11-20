FROM mcr.microsoft.com/dotnet/aspnet:9.0
LABEL name="EmailService"
COPY src/Email/bin/Release/net9.0/publish/ App/
WORKDIR /App
EXPOSE 80
ENTRYPOINT ["dotnet", "EmailService.dll"]
