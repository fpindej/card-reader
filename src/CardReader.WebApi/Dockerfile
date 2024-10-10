FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["CardReader.WebApi/CardReader.WebApi.csproj", "CardReader.WebApi/"]
RUN dotnet restore "CardReader.WebApi/CardReader.WebApi.csproj"
COPY . .
WORKDIR "/src/CardReader.WebApi"
RUN dotnet build "CardReader.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CardReader.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CardReader.WebApi.dll"]
