# Stage 1: Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution file
COPY ["TheBeautyHub.sln", "./"]

# Copy all project files
COPY ["TheBeautyHubAPI/TheBeautyHubAPI.csproj", "./TheBeautyHubAPI/"]
COPY ["TheBeautyHubData/TheBeautyHubData.csproj", "./TheBeautyHubData/"]
COPY ["TheBeautyHubCore/TheBeautyHubCore.csproj", "./TheBeautyHubCore/"]

# Restore dependencies
RUN dotnet restore "TheBeautyHub.sln"

# Copy all source code
COPY . .

# Build and publish in Release mode
RUN dotnet publish "TheBeautyHubAPI/TheBeautyHubAPI.csproj" \
    -c Release \
    -o /app/publish \
    -p:UseAppHost=false \
    -p:PublishTrimmed=false

# Stage 2: Runtime stage (smaller final image)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

# Set working directory
WORKDIR /app

# Copy published app from build stage
COPY --from=build /app/publish .

# Create non-root user for security
RUN groupadd -r dotnetuser && useradd -r -g dotnetuser dotnetuser
USER dotnetuser

# Expose port
EXPOSE 8080

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
ENV DOTNET_RUNNING_IN_CONTAINER=true

# Health check endpoint
HEALTHCHECK --interval=30s --timeout=10s --start-period=5s --retries=3 \
    CMD curl -f http://localhost:8080/health || exit 1

# Entry point
ENTRYPOINT ["dotnet", "TheBeautyHubAPI.dll"]