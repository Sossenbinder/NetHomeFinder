#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim-arm32v7 AS base
#FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app
VOLUME /storage

ENV TelegramApiToken=""
ENV TelegramChatId=""
ENV SecondsBetweenScrapes="600"
ENV ImmoscoutUrl=""

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