FROM mcr.microsoft.com/dotnet/aspnet:11.0
LABEL name="EmailService"
COPY src/bin/Release/net11.0/publish/ App/
WORKDIR /App
EXPOSE 80
ENTRYPOINT ["dotnet", "EmailService.dll"]
