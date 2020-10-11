#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
ARG ASYNC_INN_BRONZE_URL
ARG DISTRICT_MANAGER_EMAIL
ARG DISTRICT_MANAGER_PASSWORD
ARG JWT_ISSUER
ARG JWT_KEY
ARG USER_ROSE_URL

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["AsyncInnAPI.csproj", "./"]
RUN dotnet restore "./AsyncInnAPI.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "AsyncInnAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AsyncInnAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet AsyncInnAPI.dll