FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY RoutedDataPortal/RoutedDataPortal.csproj RoutedDataPortal/
COPY BusinessLibrary/BusinessLibrary.csproj BusinessLibrary/
RUN dotnet restore RoutedDataPortal/RoutedDataPortal.csproj
COPY . .
WORKDIR /src/RoutedDataPortal
RUN dotnet build RoutedDataPortal.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish RoutedDataPortal.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "RoutedDataPortal.dll"]
