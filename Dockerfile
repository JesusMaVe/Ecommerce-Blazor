FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiar archivo de solución
COPY *.sln .

# Copiar archivos de proyecto con la estructura correcta
COPY Ecommerce.App/*.csproj ./Ecommerce.App/
RUN dotnet restore "Ecommerce.App/Ecommerce.App.csproj"

# Copiar todo el código fuente
COPY . .
WORKDIR "/src/Ecommerce.App"
RUN dotnet build "./Ecommerce.App.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Ecommerce.App.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ecommerce.App.dll"]

