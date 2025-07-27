# Use the official .NET 9 SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Install Node.js (required for Angular client build)
RUN curl -fsSL https://deb.nodesource.com/setup_18.x | bash - \
    && apt-get install -y nodejs

WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["src/Web/Web.csproj", "src/Web/"]
COPY ["src/Application/Application.csproj", "src/Application/"]
COPY ["src/Domain/Domain.csproj", "src/Domain/"]
COPY ["src/Infrastructure/Infrastructure.csproj", "src/Infrastructure/"]
COPY ["Directory.Build.props", "./"]
COPY ["Directory.Packages.props", "./"]
COPY ["global.json", "./"]

RUN dotnet restore "src/Web/Web.csproj"

# Copy all source code
COPY . .

# Build and publish the application
WORKDIR "/src/src/Web"
RUN dotnet build "Web.csproj" -c Release -o /app/build
RUN dotnet publish "Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Use the official ASP.NET Core runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Create a non-root user
RUN adduser --disabled-password --gecos '' --shell /bin/bash --home /home/dotnetuser dotnetuser
RUN chown -R dotnetuser:dotnetuser /app
USER dotnetuser

EXPOSE 8080

ENTRYPOINT ["dotnet", "RestoMap.Web.dll"] 