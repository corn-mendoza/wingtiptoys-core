FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["WingtipToys.OrderManager.csproj", "."]
RUN dotnet restore "WingtipToys.OrderManager.csproj"
COPY . .
RUN dotnet build "WingtipToys.OrderManager.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "WingtipToys.OrderManager.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "WingtipToys.OrderManager.dll"]
