FROM mcr.microsoft.com/dotnet/aspnet:10.0
LABEL name="EmailService"
COPY src/Email/bin/Release/net10.0/publish/ App/
WORKDIR /App
EXPOSE 80
ENTRYPOINT ["dotnet", "EmailService.dll"]
