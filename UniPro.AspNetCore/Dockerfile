# Use the official .NET SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /app

# Copy the solution files
COPY --link UniPro.sln ./
COPY --link src/UniPro.Domain/*.csproj UniPro.Domain/
COPY --link src/UniPro.Features/*.csproj UniPro.Features/
COPY --link src/UniPro.Host/*.csproj UniPro.Host/
COPY --link src/UniPro.Infrastructure/*.csproj UniPro.Infrastructure/

# Restore dependencies
RUN dotnet restore UniPro.Host/*.csproj

# Copy everything else
COPY --link src/UniPro.Domain/ UniPro.Domain/
COPY --link src/UniPro.Features/ UniPro.Features/
COPY --link src/UniPro.Host/ UniPro.Host/
COPY --link src/UniPro.Infrastructure/ UniPro.Infrastructure/

# Builds and publishes UniPro
FROM build AS publish
WORKDIR /app/UniPro.Host
RUN dotnet publish -c Release -o /app/publish --no-restore

# Final runtime stage for running the app
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS runtime
WORKDIR /app
EXPOSE 5001

COPY --link --from=publish /app/publish .

ENTRYPOINT ["dotnet", "UniPro.Host.dll"]
