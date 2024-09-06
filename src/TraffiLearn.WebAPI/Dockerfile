FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TraffiLearn.WebAPI/TraffiLearn.WebAPI.csproj", "TraffiLearn.WebAPI/"]
COPY ["TraffiLearn.Infrastructure/TraffiLearn.Infrastructure.csproj", "TraffiLearn.Infrastructure/"]
COPY ["TraffiLearn.Application/TraffiLearn.Application.csproj", "TraffiLearn.Application/"]
COPY ["TraffiLearn.Domain/TraffiLearn.Domain.csproj", "TraffiLearn.Domain/"]
RUN dotnet restore "./TraffiLearn.WebAPI/TraffiLearn.WebAPI.csproj"
COPY . .
WORKDIR "/src/TraffiLearn.WebAPI"
RUN dotnet build "./TraffiLearn.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TraffiLearn.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TraffiLearn.WebAPI.dll"]