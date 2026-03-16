# build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["SistemaLavanderia.csproj", "./"]
RUN dotnet restore "./SistemaLavanderia.csproj"

COPY . .
RUN dotnet publish "SistemaLavanderia.csproj" -c Release -o /app/publish /p:UseAppHost=false

# runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://0.0.0.0:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "SistemaLavanderia.dll"]