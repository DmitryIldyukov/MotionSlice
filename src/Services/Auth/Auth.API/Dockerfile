FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["src/Services/Auth/Auth.API/Auth.API.csproj", "src/Services/Auth/Auth.API/"]
COPY ["src/Services/Auth/Auth.Application/Auth.Application.csproj", "src/Services/Auth/Auth.Application/"]
COPY ["src/Services/Auth/Auth.Domain/Auth.Domain.csproj", "src/Services/Auth/Auth.Domain/"]
COPY ["src/Services/Auth/Auth.Infrastructure/Auth.Infrastructure.csproj", "src/Services/Auth/Auth.Infrastructure/"]
RUN dotnet restore "src/Services/Auth/Auth.API/Auth.API.csproj"

COPY src/Services/Auth/ src/Services/Auth/
RUN dotnet publish "src/Services/Auth/Auth.API/Auth.API.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

USER $APP_UID

ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Auth.API.dll"]
