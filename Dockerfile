FROM mcr.microsoft.com/dotnet/aspnet:5.0
LABEL name="EmailService"
COPY src/Email/bin/Release/net5.0/publish/ App/
WORKDIR /App
EXPOSE 80
ENTRYPOINT ["dotnet", "EmailService.dll"]
