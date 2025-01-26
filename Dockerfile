FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5004

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Src/HotelUp.Cleaning.API/HotelUp.Cleaning.API.csproj", "Src/HotelUp.Cleaning.API/"]
COPY ["Src/HotelUp.Cleaning.Services/HotelUp.Cleaning.Services.csproj", "Src/HotelUp.Cleaning.Services/"]
COPY ["Src/HotelUp.Cleaning.Persistence/HotelUp.Cleaning.Persistence.csproj", "Src/HotelUp.Cleaning.Persistence/"]
COPY ["Shared/HotelUp.Cleaning.Shared/HotelUp.Cleaning.Shared.csproj", "Shared/HotelUp.Cleaning.Shared/"]
RUN dotnet restore "Src/HotelUp.Cleaning.API/HotelUp.Cleaning.API.csproj"
COPY . .
WORKDIR "/src/Src/HotelUp.Cleaning.API"
RUN dotnet build "HotelUp.Cleaning.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "HotelUp.Cleaning.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
    CMD curl --silent --fail http://localhost:5004/api/cleaning/_health || exit 1

ENTRYPOINT ["dotnet", "HotelUp.Cleaning.API.dll"]
