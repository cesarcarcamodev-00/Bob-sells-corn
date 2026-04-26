FROM mcr.microsoft.com/dotnet/sdk:10.0
WORKDIR /app
COPY . .
RUN dotnet restore && dotnet publish -c Release -o ./publish
EXPOSE 5180
WORKDIR /app/publish
ENTRYPOINT ["dotnet", "Bob-sells-corn.API.dll"]