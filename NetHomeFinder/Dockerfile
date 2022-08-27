#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim-arm32v7 AS base
FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app
VOLUME /storage

ENV TelegramApiToken="1027531170:AAEk3yzzjVaghnkL_G7MF4eHsckO_KhJJ-s"
ENV TelegramChatId="-1001157116554"
ENV SecondsBetweenScrapes="10800"
ENV ImmoscoutUrl="https://www.immobilienscout24.de/Suche/de/baden-wuerttemberg/esslingen-kreis/wohnung-mieten?price=-800.0&geocodes=1276001010031,1276001010014,1276001010043,1276001010010,1276001010042,1276001010038,1276001010037,1276001010020,1276001010003,1276001010018&sorting=2"

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["NetHomeFinder/NetHomeFinder.csproj", "NetHomeFinder/"]
RUN dotnet restore "NetHomeFinder/NetHomeFinder.csproj"
COPY . .
WORKDIR "/src/NetHomeFinder"
RUN dotnet build "NetHomeFinder.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NetHomeFinder.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NetHomeFinder.dll"]