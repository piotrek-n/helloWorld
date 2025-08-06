# Use the official .NET 9.0 runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Set environment variables for Azure
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production

# Use the .NET 9.0 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["BlazorApp1.csproj", "."]
RUN dotnet restore "BlazorApp1.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "BlazorApp1.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BlazorApp1.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Add health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
  CMD curl -f http://localhost:80/ || exit 1

ENTRYPOINT ["dotnet", "BlazorApp1.dll"]
