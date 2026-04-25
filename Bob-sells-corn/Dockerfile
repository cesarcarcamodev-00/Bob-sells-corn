# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS builder
WORKDIR /src

COPY Bob-sells-corn.API.csproj ./Bob-sells-corn/
COPY Bob-sells-corn/*.csproj ./Bob-sells-corn/
RUN dotnet restore Bob-sells-corn/Bob-sells-corn.API.csproj

COPY Bob-sells-corn/ ./Bob-sells-corn/
RUN dotnet publish Bob-sells-corn/Bob-sells-corn.API.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=builder /app/publish .

EXPOSE 5180
ENTRYPOINT ["dotnet", "Bob-sells-corn.API.dll"]