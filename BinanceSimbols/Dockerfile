#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["BinanceSimbols/BinanceSimbols.csproj", "."]
RUN dotnet restore "./BinanceSimbols.csproj"
COPY BinanceSimbols .
WORKDIR "/src/."
RUN dotnet build "BinanceSimbols.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BinanceSimbols.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BinanceSimbols.dll"]