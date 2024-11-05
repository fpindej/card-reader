# Use the official ASP.NET Core runtime as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ["CardReader.WebApi/CardReader.WebApi.csproj", "CardReader.WebApi/"]
RUN dotnet restore "CardReader.WebApi/CardReader.WebApi.csproj"

# Copy the entire source code
COPY . .

# Build the application
WORKDIR "/src/CardReader.WebApi"
RUN dotnet build "CardReader.WebApi.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "CardReader.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false /p:DebugType=None /p:DebugSymbols=false

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "CardReader.WebApi.dll"]